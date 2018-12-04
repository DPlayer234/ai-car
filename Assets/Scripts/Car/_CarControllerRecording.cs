using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using DataSets = System.Collections.Generic.List<float[]>;

namespace DPlay.AICar.Car
{
    public class CarControllerRecording : CarController
    {
        public float[] RayCastAngles = { -55, 0, 55 };

        public float MaximumRayCastDistance = 10.0f;

        public Vector3 RayCastOriginOffset = new Vector3(0.0f, 0.5f, 0.0f);

        public int DataSetCount;

        public DataSets DataSets = new DataSets();

        public bool LoadPreviousData = false;

        public string DataSetOutputFileName = "CarControllerTrainingData";

        public float AutoSaveDelay = 5.0f;

        private string DataSetOutputPath;

        protected void RecordRayCasts()
        {
            float[] set = new float[RayCastAngles.Length + 3];

            for (int i = 0; i < RayCastAngles.Length; i++)
            {
                float angle = RayCastAngles[i];
                Vector3 direction = HelperFunctions.RotateAroundY(transform.forward, angle);

                RaycastHit hitInfo;

                set[i] =
                    Physics.Raycast(transform.position + RayCastOriginOffset, direction, out hitInfo, MaximumRayCastDistance)
                    ? hitInfo.distance
                    : MaximumRayCastDistance;
            }

            // This is always 0
            set[RayCastAngles.Length + 0] = 1.0f;

            // Need the inputs as well.
            set[RayCastAngles.Length + 1] = GetLinearSpeedInput();
            set[RayCastAngles.Length + 2] = GetAngularSpeedInput();

            DataSets.Add(set);
            DataSetCount = DataSets.Count;
        }

        protected override void Awake()
        {
            DataSetOutputPath = HelperFunctions.GetPathTo(DataSetOutputFileName);
            Debug.Log("Saving Training Data to: " + DataSetOutputPath);

            base.Awake();
        }

        protected override void FixedUpdate()
        {
            RecordRayCasts();

            base.FixedUpdate();
        }
    }
}

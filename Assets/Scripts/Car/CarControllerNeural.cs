using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DPlay.AICar.MachineLearning;

namespace DPlay.AICar.Car
{
    public class CarControllerNeural : CarController
    {
        public float[] RayCastAngles = { -35, 0, 35, 180 };

        public float MaximumRayCastDistance = 10.0f;

        public Vector3 RayCastOriginOffset = new Vector3(0.0f, 0.5f, 0.0f);
        
        public NeuralNet Brain;

        public INeuron LinearSpeedOutput => Brain.Outputs[0];

        public INeuron AngularSpeedOutput => Brain.Outputs[1];

        public override float GetLinearSpeedInput()
        {
            return (float)LinearSpeedOutput.Predict();
        }

        public override float GetAngularSpeedInput()
        {
            return (float)AngularSpeedOutput.Predict();
        }

        protected void RecordRayCasts()
        {
            for (int i = 0; i < RayCastAngles.Length; i++)
            {
                float angle = RayCastAngles[i];
                Vector3 direction = HelperFunctions.RotateAroundY(transform.forward, angle);

                RaycastHit hitInfo;
                
                Brain.Inputs[i].PredictedValue =
                    Physics.Raycast(transform.position + RayCastOriginOffset, direction, out hitInfo, MaximumRayCastDistance)
                    ? hitInfo.distance
                    : MaximumRayCastDistance;
            }
        }
        
        protected void InitializeBrain()
        {
            int inputCount = RayCastAngles.Length;
            Brain = new NeuralNet(inputCount, inputCount, 2);

            Brain.Layers[1].SetAllActivationFunctions(ActivationFunctions.Tanh);

            HelperFunctions.DebugLogArray(Brain.GetAllWeights());
        }

        protected override void Awake()
        {
            base.Awake();

            InitializeBrain();
        }

        protected override void FixedUpdate()
        {
            RecordRayCasts();

            base.FixedUpdate();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            for (int i = 0; i < RayCastAngles.Length; i++)
            {
                float angle = RayCastAngles[i];
                Vector3 direction = HelperFunctions.RotateAroundY(transform.forward, angle);

                Gizmos.DrawRay(transform.position + RayCastOriginOffset, direction * MaximumRayCastDistance);
            }
        }
    }
}

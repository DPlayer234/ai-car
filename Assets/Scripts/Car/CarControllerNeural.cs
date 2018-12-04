using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DPlay.AICar.MachineLearning;
using DPlay.AICar.MachineLearning.Evolution;

namespace DPlay.AICar.Car
{
    public class CarControllerNeural : CarController, IEvolvable
    {
        public float[] RayCastAngles = { -35, 0, 35, 180 };

        public float MaximumRayCastDistance = 10.0f;

        public Vector3 RayCastOriginOffset = new Vector3(0.0f, 0.5f, 0.0f);

        public LayerMask RayCastLayerMask;

        public bool DisableOnCrash = false;

        public string DisableOnCrashWithTag = "Barrier";

        public bool DrawRayCastGizmos = false;

        public double Fitness { get; protected set; }
        
        public NeuralNet Brain { get; protected set; }

        public INeuron LinearSpeedOutput => Brain.Outputs[0];

        public INeuron AngularSpeedOutput => Brain.Outputs[1];

        bool IEvolvable.Active => enabled;

        public override float GetLinearSpeedInput()
        {
            return (float)LinearSpeedOutput.PredictedValue;
        }

        public override float GetAngularSpeedInput()
        {
            return (float)AngularSpeedOutput.PredictedValue;
        }

        public double[] GetGenome()
        {
            return Brain.GetAllWeights();
        }

        public void SetGenome(double[] genes)
        {
            Brain.SetAllWeights(genes);
        }

        protected void RecordRayCasts()
        {
            for (int i = 0; i < RayCastAngles.Length; i++)
            {
                float angle = RayCastAngles[i];
                Vector3 direction = HelperFunctions.RotateAroundY(transform.forward, angle);

                RaycastHit hitInfo;
                
                Brain.Inputs[i].PredictedValue =
                    Physics.Raycast(transform.position + RayCastOriginOffset, direction, out hitInfo, MaximumRayCastDistance, RayCastLayerMask.value)
                    ? hitInfo.distance
                    : MaximumRayCastDistance;
            }

            Brain.Predict();
        }
        
        protected void InitializeBrain()
        {
            int inputCount = RayCastAngles.Length;
            Brain = new NeuralNet(inputCount, inputCount, 2);

            Brain.OutputLayer.SetAllActivationFunctions(ActivationFunctions.Tanh);
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

            Fitness += LinearSpeed * 10.0f * Time.fixedDeltaTime;
        }

        private void OnDrawGizmos()
        {
            if (!DrawRayCastGizmos) return;

            Gizmos.color = Color.cyan;

            for (int i = 0; i < RayCastAngles.Length; i++)
            {
                float angle = RayCastAngles[i];
                Vector3 direction = HelperFunctions.RotateAroundY(transform.forward, angle);

                Gizmos.DrawRay(transform.position + RayCastOriginOffset, direction * MaximumRayCastDistance);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag(DisableOnCrashWithTag))
            {
                enabled = false;

                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }
        }

        int IComparable<IEvolvable>.CompareTo(IEvolvable other)
        {
            return -Fitness.CompareTo(other.Fitness);
        }
    }
}

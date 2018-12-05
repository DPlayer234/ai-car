using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DPlay.AICar.MachineLearning;
using DPlay.AICar.MachineLearning.Evolution;

namespace DPlay.AICar.Car
{
    /// <summary>
    ///     Implements behavior necessary to allow a <seealso cref="NeuralNet"/> controlled <seealso cref="CarController"/> to evolve.
    /// </summary>
    public class CarControllerNeural : CarController, IEvolvable
    {
        /// <summary> Angles for the input ray-casts. </summary>
        public float[] RayCastAngles = { -35, 0, 35, 180 };

        /// <summary> The maximum distance the raycasts are sent out. </summary>
        public float MaximumRayCastDistance = 15.0f;

        /// <summary> The offset for the ray-cast origin. </summary>
        public Vector3 RayCastOriginOffset = new Vector3(0.0f, 0.5f, 0.0f);

        /// <summary> The layer mask to select what may be considered by the ray-casts. </summary>
        public LayerMask RayCastLayerMask;

        /// <summary> Whether or not to disable this behavior on a crash. </summary>
        public bool DisableOnCrash = false;

        /// <summary> The tag that the collided object has to have to cause a deactivation. </summary>
        public string BarrierTag = "Barrier";

        /// <summary> Whether or not to draw the ray-casts as gizmos in the editor. </summary>
        public bool DrawRayCastGizmos = false;

        /// <summary>
        ///     The Fitness of the car, aka how "good" it is.
        /// </summary>
        public double Fitness { get; protected set; }
        
        /// <summary>
        ///     The "Brain" of the car dictating the behavior.
        /// </summary>
        public NeuralNet Brain { get; protected set; }

        /// <summary>
        ///     The output <seealso cref="INeuron"/> for the linear speed input.
        /// </summary>
        public INeuron LinearSpeedOutput => Brain.Outputs[0];

        /// <summary>
        ///     The output <seealso cref="INeuron"/> for the angular speed input.
        /// </summary>
        public INeuron AngularSpeedOutput => Brain.Outputs[1];

        /// <summary>
        ///     Gets the linear speed input.
        /// </summary>
        /// <returns>The input value.</returns>
        public override float GetLinearSpeedInput()
        {
            return (float)LinearSpeedOutput.PredictedValue;
        }

        /// <summary>
        ///     Gets the angular speed input.
        /// </summary>
        /// <returns>The input value.</returns>
        public override float GetAngularSpeedInput()
        {
            return (float)AngularSpeedOutput.PredictedValue;
        }

        /// <summary>
        ///     Gets the current genome.
        /// </summary>
        /// <returns>An array of <seealso cref="double"/>s, representing the genome.</returns>
        public double[] GetGenome()
        {
            return Brain.GetAllWeights();
        }

        /// <summary>
        ///     Overrides the current genome.
        /// </summary>
        /// <param name="genes">An array of <seealso cref="double"/>s, representing the new genome.</param>
        /// <exception cref="ArgumentException">Amount of genes supplied does not match amount of genes expected.</exception>
        public void SetGenome(double[] genes)
        {
            Brain.SetAllWeights(genes);
        }

        /// <summary>
        ///     Records the ray-casts, writes their distances to the <seealso cref="Brain"/>'s input nerves and then calculates the predictions.
        /// </summary>
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
        
        /// <summary>
        ///     Creates and initializes the brain.
        ///     The output neurons have their Activation Functions set to <see cref="ActivationFunctions.Tanh"/>.
        /// </summary>
        protected void InitializeBrain()
        {
            int inputCount = RayCastAngles.Length;
            Brain = new NeuralNet(inputCount, inputCount, 2);

            Brain.OutputLayer.SetAllActivationFunctions(ActivationFunctions.Tanh);
        }

        /// <summary>
        ///     Called by Unity once to initialize the <seealso cref="CarControllerNeural"/>.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            InitializeBrain();
        }

        /// <summary>
        ///     Called by Unity to update the <seealso cref="CarControllerNeural"/> every fixed update step.
        /// </summary>
        protected override void FixedUpdate()
        {
            RecordRayCasts();

            base.FixedUpdate();

            Fitness += LinearSpeed * 10.0f * Time.fixedDeltaTime;
        }

        /// <summary>
        ///     Called by Unity to draw gizmos in the editor.
        /// </summary>
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

        /// <summary>
        ///     Called by Unity when a collision occurs.
        /// </summary>
        /// <param name="collision">The collision that occurred.</param>
        private void OnCollisionEnter(Collision collision)
        {
            if (DisableOnCrash && collision.collider.CompareTag(BarrierTag))
            {
                enabled = false;

                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }
        }

        /// <summary>
        ///     Implements the <seealso cref="IComparable{IEvolvable}"/> interface.
        ///     Compares own <see cref="IEvolvable.Fitness"/> with that of another <see cref="IEvolvable"/>.
        /// </summary>
        /// <param name="other">The other <see cref="IEvolvable"/> to compare to.</param>
        /// <returns>The comparison result.</returns>
        int IComparable<IEvolvable>.CompareTo(IEvolvable other)
        {
            return -Fitness.CompareTo(other.Fitness);
        }
    }
}

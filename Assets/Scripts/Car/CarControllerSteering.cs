using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DPlay.AICar.Car
{
    /// <summary>
    ///     Controls the car based on a given path via steering behavior.
    /// </summary>
    public class CarControllerSteering : CarController
    {
        /// <summary> The distance forward to use to detect the next closest path segment </summary>
        public float PredictionDistance = 2.5f;

        /// <summary> The distance along the path segment to add </summary>
        public float FutureDistance = 2.5f;

        /// <summary> The multiplier to apply to linear speed inputs </summary>
        public float LinearSpeedMultiplier = 1.0f;

        /// <summary> The multiplier to apply to angular speed inputs </summary>
        public float AngularSpeedMultiplier = 0.1f;

        /// <summary> The path this car is supposed to follow </summary>
        public Path Path;

        /// <summary> Stores the computed value for the linear speed input </summary>
        private float linearSpeedInput;

        /// <summary> Stores the computed value for the angular speed input </summary>
        private float angularSpeedInput;

        /// <summary>
        ///     Gets the linear speed input.
        /// </summary>
        /// <returns>The input value.</returns>
        public override float GetLinearSpeedInput()
        {
            return Mathf.Clamp(linearSpeedInput, -1.0f, 1.0f); ;
        }

        /// <summary>
        ///     Gets the angular speed input.
        /// </summary>
        /// <returns>The input value.</returns>
        public override float GetAngularSpeedInput()
        {
            return Mathf.Clamp(angularSpeedInput, -1.0f, 1.0f); ;
        }

        /// <summary>
        ///     Called by Unity to initialize the <see cref="CarControllerSteering"/>.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        ///     Called by Unity to update the <see cref="CarControllerSteering"/> every fixed update.
        /// </summary>
        protected override void FixedUpdate()
        {
            Vector3 targetPosition = ComputeTargetPosition();

            // Get the position and angle offset
            Vector3 desiredDirection = targetPosition - transform.position;
            float angleOffset = Vector3.Angle(transform.forward, desiredDirection);

            // Make sure angle is signed (to the right, positive; to the left, negative)
            bool right = Vector3.Angle(transform.right, desiredDirection) < Vector3.Angle(-transform.right, desiredDirection);
            if (!right) angleOffset *= -1;

            linearSpeedInput = desiredDirection.magnitude / (PredictionDistance + FutureDistance) * LinearSpeedMultiplier;
            angularSpeedInput = angleOffset * AngularSpeedMultiplier;

            base.FixedUpdate();
        }

        /// <summary>
        ///     Computes the target position to drive to now and returns it.
        /// </summary>
        /// <returns>The target position</returns>
        private Vector3 ComputeTargetPosition()
        {
            Vector3 predictedPos = transform.position + rigidbody.velocity.normalized * PredictionDistance;
            PathSegment segment = Path.GetClosestSegment(predictedPos);
            Vector3 normalPos = segment.GetNormalPointTo(predictedPos);
            Vector3 nextOffset = segment.Direction * FutureDistance;

            return Vector3.Distance(predictedPos, normalPos) > Path.Radius
                ? normalPos + nextOffset
                : transform.position + nextOffset;
        }
    }
}

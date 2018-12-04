using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DPlay.AICar.Car
{
    public class CarController : MonoBehaviour
    {
        public float MaximumLinearSpeed = 8.0f;

        public float MaximumAngularSpeed = 2.0f;

        [Range(0.0f, 0.95f)]
        public float LinearAccelerationFactor = 0.1f;

        [Range(0.0f, 0.95f)]
        public float AngularAccelerationFactor = 0.1f;

        new protected Rigidbody rigidbody;

        public float LinearSpeed => Vector3.Dot(rigidbody.velocity, transform.forward);

        public virtual float GetLinearSpeedInput()
        {
            return Input.GetAxis("Vertical");
        }

        public virtual float GetAngularSpeedInput()
        {
            return Input.GetAxis("Horizontal");
        }

        public float GetProcessedLinearSpeedInput()
        {
            return Mathf.Max(0.0f, GetLinearSpeedInput());
        }

        public float GetProcessedAngularSpeedInput()
        {
            return Mathf.Min(1.0f, 2.0f * Mathf.Abs(LinearSpeed) / MaximumLinearSpeed) * GetAngularSpeedInput();
        }

        public void Accelerate(float deltaTime)
        {
            rigidbody.velocity = HelperFunctions.ExpApproach(
                rigidbody.velocity,  // Current Velocity
                MaximumLinearSpeed * GetProcessedLinearSpeedInput() * transform.forward,  // Target Velocity
                LinearAccelerationFactor,
                deltaTime);

            rigidbody.angularVelocity = HelperFunctions.ExpApproach(
                rigidbody.angularVelocity,  // Current Angular Velocity
                MaximumAngularSpeed * GetProcessedAngularSpeedInput() * Vector3.up,  // Target Angular Velocity
                AngularAccelerationFactor,
                deltaTime);
        }

        protected virtual void Awake()
        {
            this.FetchComponent(ref rigidbody);
        }

        protected virtual void FixedUpdate()
        {
            Accelerate(Time.fixedDeltaTime);
        }
    }
}

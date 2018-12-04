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

        new private Rigidbody rigidbody;

        public virtual float GetLinearSpeedInput()
        {
            return Input.GetAxis("Vertical");
        }

        public virtual float GetAngularSpeedInput()
        {
            return Input.GetAxis("Horizontal");
        }

        public void Accelerate(float deltaTime)
        {
            rigidbody.velocity = HelperFunctions.ExpApproach(
                rigidbody.velocity,  // Current Velocity
                MaximumLinearSpeed * GetLinearSpeedInput() * transform.forward,  // Target Velocity
                LinearAccelerationFactor,
                deltaTime);

            rigidbody.angularVelocity = HelperFunctions.ExpApproach(
                rigidbody.angularVelocity,  // Current Angular Velocity
                MaximumAngularSpeed * GetAngularSpeedInput() * Vector3.up,  // Target Angular Velocity
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

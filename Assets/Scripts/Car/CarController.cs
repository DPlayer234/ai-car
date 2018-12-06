using UnityEngine;

namespace DPlay.AICar.Car
{
    /// <summary>
    ///     Implements basic behavior to have a controllable car.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [DisallowMultipleComponent]
    public abstract class CarController : MonoBehaviour
    {
        /// <summary> The maximum recognized input forward. </summary>
        public const float MaximumForwardInput = 1.0f;
        
        /// <summary> The minimum recognized input backward. </summary>
        public const float MinimumBackwardInput = -0.15f;

        /// <summary> The maximum linear speed the car may have going forward. </summary>
        public float MaximumLinearSpeed = 8.0f;

        /// <summary> The maximum angular speed the car may have, meaning how strong it can steer to the sides. </summary>
        public float MaximumAngularSpeed = 2.0f;

        /// <summary> How fast the car accelerates. Range: [0.0..1.0] (Low values are faster) </summary>
        [Range(0.0f, 0.95f)]
        public float LinearAccelerationFactor = 0.1f;

        /// <summary> How well the car steers. Range: [0.0..1.0] (Low values are faster) </summary>
        [Range(0.0f, 0.95f)]
        public float AngularAccelerationFactor = 0.1f;

        /// <summary> The attached rigidbody component. </summary>
        new protected Rigidbody rigidbody;

        /// <summary> How fast the car is going along the <seealso cref="transform.forward"/> axis. </summary>
        public float LinearSpeed => Vector3.Dot(rigidbody.velocity, transform.forward);

        /// <summary>
        ///     Gets the linear speed input. This should be in range [-1.0..1.0] to not cause unexpected behavior.
        /// </summary>
        /// <returns>The input value.</returns>
        public abstract float GetLinearSpeedInput();

        /// <summary>
        ///     Gets the angular speed input. This should be in range [-1.0..1.0] to not cause unexpected behavior.
        /// </summary>
        /// <returns>The input value.</returns>
        public abstract float GetAngularSpeedInput();

        /// <summary>
        ///     Gets the processed linear input. Clamps to [0.0..1.0].
        /// </summary>
        /// <returns>The processed input value.</returns>
        public float GetProcessedLinearSpeedInput()
        {
            return Mathf.Clamp(GetLinearSpeedInput(), MinimumBackwardInput, MaximumForwardInput);
        }

        /// <summary>
        ///     Gets the processed angular input. The actual output is dedicated by how fast the car is going.
        /// </summary>
        /// <returns>The processed input value.</returns>
        public float GetProcessedAngularSpeedInput()
        {
            // Basically reduces the angular input if the velocity of the car is below half of MaximumLinearSpeed
            return Mathf.Min(1.0f, 2.0f * Mathf.Abs(LinearSpeed) / MaximumLinearSpeed) * GetAngularSpeedInput();
        }

        /// <summary>
        ///     Accelerates the car based on the inputs.
        ///     Should be called in FixedUpdate().
        /// </summary>
        /// <param name="deltaTime">The time passed since the last update (e.g. <seealso cref="Time.fixedDeltaTime"/>)</param>
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

        /// <summary>
        ///     Called by Unity once to initialize the <seealso cref="CarController"/>.
        /// </summary>
        protected virtual void Awake()
        {
            this.FetchComponent(ref rigidbody);
        }

        /// <summary>
        ///     Called by Unity to update the <seealso cref="CarController"/> every fixed update step.
        /// </summary>
        protected virtual void FixedUpdate()
        {
            Accelerate(Time.fixedDeltaTime);
        }
    }
}

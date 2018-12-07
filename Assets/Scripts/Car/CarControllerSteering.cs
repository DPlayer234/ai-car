using DPlay.AICar.SteeringBehavior;
using UnityEngine;

namespace DPlay.AICar.Car
{
    /// <summary>
    ///     Controls the car based on a custom algorithm and steering behavior.
    /// </summary>
    public partial class CarControllerSteering : CarController
    {
        /// <summary> The offset for the ray-cast origin. </summary>
        public Vector3 RayCastOriginOffset = new Vector3(0.0f, 0.5f, 0.0f);

        /// <summary> The layer mask to select what may be considered by the ray-casts. </summary>
        public LayerMask RayCastLayerMask;

        /// <summary> The maximum distance the raycasts are sent out. </summary>
        private const float MaximumRayCastDistance = 15.0f;

        /// <summary> The angle for the ray cast going forwards. </summary>
        private const float ForwardsAngle = 0;

        /// <summary> The angle for the ray cast going half-left. </summary>
        private const float HalfLeftAngle = -25;

        /// <summary> The angle for the ray cast going half-right. </summary>
        private const float HalfRightAngle = 25;

        /// <summary> The angle for the ray cast going left. </summary>
        private const float LeftAngle = -80;

        /// <summary> The angle for the ray cast going right. </summary>
        private const float RightAngle = 80;

        /// <summary> The angle for the ray cast going backwards. </summary>
        private const float BackwardsAngle = 180;

        /// <summary> The multiplier for the angular speed input based on half-left/right angles. </summary>
        private const float HalfAngularInputMultiplier = 7.0f;

        /// <summary> The multiplier for the angular speed input based on left/right angles. </summary>
        private const float AngularInputMultiplier = 2.0f;

        /// <summary> The used FSM. </summary>
        private FSM<CarControllerSteering> accelerationStateMachine;

        /// <summary> The distance to the next barrier along the ray of <see cref="ForwardsAngle"/>. </summary>
        private float forwardsDistance;

        /// <summary> The distance to the next barrier along the ray of <see cref="HalfLeftAngle"/>. </summary>
        private float halfLeftDistance;

        /// <summary> The distance to the next barrier along the ray of <see cref="HalfRightAngle"/>. </summary>
        private float halfRightDistance;

        /// <summary> The distance to the next barrier along the ray of <see cref="LeftAngle"/>. </summary>
        private float leftDistance;

        /// <summary> The distance to the next barrier along the ray of <see cref="RightAngle"/>. </summary>
        private float rightDistance;

        /// <summary> The distance to the next barrier along the ray of <see cref="BackwardsAngle"/>. </summary>
        private float backwardsDistance;

        /// <summary> Computed value for <see cref="GetLinearSpeedInput"/> </summary>
        private float linearSpeedInput;

        /// <summary> Computed value for <see cref="GetAngularSpeedInput"/> </summary>
        private float angularSpeedInput;

        /// <summary>
        ///     Gets the linear speed input.
        /// </summary>
        /// <returns>The input value.</returns>
        public override float GetLinearSpeedInput()
        {
            return linearSpeedInput;
        }

        /// <summary>
        ///     Gets the angular speed input.
        /// </summary>
        /// <returns>The input value.</returns>
        public override float GetAngularSpeedInput()
        {
            return angularSpeedInput;
        }

        /// <summary>
        ///     Records a single raycast and returns the distance to the next wall.
        /// </summary>
        /// <param name="angle">The angle of the ray relative to the current facing direction.</param>
        /// <returns>The distance to the next wall or <seealso cref="Globals.LargeDistance"/> if nothing is hit.</returns>
        protected float RecordRayCast(float angle)
        {
            Vector3 direction = HelperFunctions.RotateAroundY(transform.forward, angle);

            RaycastHit hitInfo;

            return Physics.Raycast(transform.position + RayCastOriginOffset, direction, out hitInfo, MaximumRayCastDistance, RayCastLayerMask.value)
                ? hitInfo.distance
                : Globals.LargeDistance;
        }

        /// <summary>
        ///     Records the current ray cast distances.
        /// </summary>
        protected void RecordRayCasts()
        {
            forwardsDistance = RecordRayCast(ForwardsAngle);

            halfLeftDistance = RecordRayCast(HalfLeftAngle);
            halfRightDistance = RecordRayCast(HalfRightAngle);

            leftDistance = RecordRayCast(LeftAngle);
            rightDistance = RecordRayCast(RightAngle);

            backwardsDistance = RecordRayCast(BackwardsAngle);
        }

        /// <summary>
        ///     Computes the required inputs based on the current ray cast distances.
        /// </summary>
        protected void ComputeInputs()
        {
            angularSpeedInput =
                (rightDistance - leftDistance) * AngularInputMultiplier +
                (halfRightDistance - halfLeftDistance) * HalfAngularInputMultiplier;

            accelerationStateMachine.Update();

            linearSpeedInput = Mathf.Clamp(linearSpeedInput, -1.0f, 1.0f);
            angularSpeedInput = Mathf.Clamp(angularSpeedInput, -1.0f, 1.0f);
        }

        /// <summary>
        ///     Called by Unity to initialize the <see cref="CarControllerSteering"/>.
        /// </summary>
        protected override void Awake()
        {
            InitializeFSM();

            base.Awake();
        }

        /// <summary>
        ///     Called by Unity to update the <see cref="CarControllerSteering"/> every fixed update.
        /// </summary>
        protected override void FixedUpdate()
        {
            RecordRayCasts();
            ComputeInputs();

            base.FixedUpdate();
        }

        /// <summary>
        ///     Initializes the <see cref="accelerationStateMachine"/>.
        /// </summary>
        private void InitializeFSM()
        {
            accelerationStateMachine = new FSM<CarControllerSteering>(this);

            var forwards = new StateForward();
            var backwards = new StateBackwards();

            accelerationStateMachine.ActiveState = forwards;

            accelerationStateMachine.AddTransition(forwards, backwards);
            accelerationStateMachine.AddTransition(backwards, forwards);
        }
    }
}

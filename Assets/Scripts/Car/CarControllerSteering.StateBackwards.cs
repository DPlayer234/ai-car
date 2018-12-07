using DPlay.AICar.SteeringBehavior;

namespace DPlay.AICar.Car
{
    /// <summary>
    ///     Controls the car based on a custom algorithm and steering behavior.
    /// </summary>
    public partial class CarControllerSteering
    {
        /// <summary>
        ///     FSM State for driving backwards.
        /// </summary>
        private class StateBackwards : FSM<CarControllerSteering>.IStateTo<StateForward>
        {
            /// <summary> The distance to a wall that is considered far enough to start driving forwards again. </summary>
            private const float NotCloseToWall = 3.5f;

            /// <summary> The multiplier for the linear speed input. </summary>
            private const float LinearInputMultiplier = 0.1f;

            /// <summary> The <see cref="CarControllerSteering"/> this state is for. </summary>
            public CarControllerSteering Self { get; set; }

            /// <summary>
            ///     Called every update while active. Sets the linear speed input.
            /// </summary>
            public void Update()
            {
                Self.linearSpeedInput = -Self.backwardsDistance * LinearInputMultiplier;
            }

            /// <summary>
            ///     Indicates whether a transition to <see cref="StateForward"/> is allowed.
            /// </summary>
            /// <returns>A boolean indicating whether the transition may occur.</returns>
            bool FSM<CarControllerSteering>.IStateTo<StateForward>.MayTransition()
            {
                return Self.forwardsDistance > NotCloseToWall;
            }
        }
    }
}

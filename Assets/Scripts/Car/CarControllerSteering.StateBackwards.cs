using DPlay.AICar.SteeringBehavior;

namespace DPlay.AICar.Car
{
    /// <summary>
    ///     Controls the car based on a custom algorithm.
    /// </summary>
    public partial class CarControllerSteering
    {
        /// <summary>
        ///     FSM State for driving backwards.
        /// </summary>
        private class StateBackwards : FiniteStateMachine.IState<StateForward>
        {
            /// <summary> The distance to a wall that is considered far enough to start driving forwards again. </summary>
            private const float NotCloseToWall = 3.5f;

            /// <summary> The source object that the related FSM belongs to. </summary>
            private CarControllerSteering self;

            /// <summary>
            ///     Initializes a new instance of the <see cref="CarControllerSteering"/> class.
            /// </summary>
            /// <param name="self">The source object that the related FSM belongs to.</param>
            public StateBackwards(CarControllerSteering self)
            {
                this.self = self;
            }

            /// <summary>
            ///     Called when the state is entered/set as active. Does nothing.
            /// </summary>
            public void Enter() { }

            /// <summary>
            ///     Called when the state is exitted/set as not active. Does nothing.
            /// </summary>
            public void Exit() { }

            /// <summary>
            ///     Called every update while active. Sets the linear speed input.
            /// </summary>
            public void Update()
            {
                self.linearSpeedInput = -self.backwardsDistance * LinearInputMultiplier;
            }

            /// <summary>
            ///     Indicates whether a transition to <see cref="StateForward"/> is allowed.
            /// </summary>
            /// <returns>A boolean indicating whether the transition may occur.</returns>
            bool FiniteStateMachine.IState<StateForward>.MayTransition()
            {
                return self.forwardsDistance > NotCloseToWall;
            }
        }
    }
}

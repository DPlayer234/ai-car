using DPlay.AICar.SteeringBehavior;
using UnityEngine;

namespace DPlay.AICar.Car
{
    /// <summary>
    ///     Controls the car based on a custom algorithm.
    /// </summary>
    public partial class CarControllerSteering
    {
        private class StateBackwards : FiniteStateMachine.IState<StateForward>
        {
            /// <summary> The distance to a wall that is considered far enough to start driving forwards again. </summary>
            private const float NotCloseToWall = 3.5f;

            private CarControllerSteering self;

            public StateBackwards(CarControllerSteering self)
            {
                this.self = self;
            }

            public void Enter() { }

            public void Exit() { }

            public void Update()
            {
                self.linearSpeedInput = -self.backwardsDistance * LinearInputMultiplier;
            }

            bool FiniteStateMachine.IState<StateForward>.CanTransition()
            {
                return self.forwardsDistance > NotCloseToWall;
            }
        }
    }
}

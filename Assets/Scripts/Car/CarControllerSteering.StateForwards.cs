using DPlay.AICar.SteeringBehavior;
using UnityEngine;

namespace DPlay.AICar.Car
{
    /// <summary>
    ///     Controls the car based on a custom algorithm.
    /// </summary>
    public partial class CarControllerSteering
    {
        private class StateForward : FiniteStateMachine.IState<StateBackwards>
        {
            /// <summary> The distance to a wall that is considered close. </summary>
            private const float CloseToWall = 2.5f;

            private CarControllerSteering self;

            public StateForward(CarControllerSteering self)
            {
                this.self = self;
            }

            public void Enter() { }

            public void Exit() { }

            public void Update()
            {
                self.linearSpeedInput = self.forwardsDistance * LinearInputMultiplier;
            }

            bool FiniteStateMachine.IState<StateBackwards>.CanTransition()
            {
                return self.forwardsDistance < CloseToWall;
            }
        }
    }
}

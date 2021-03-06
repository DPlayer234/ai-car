﻿using DPlay.AICar.FiniteStateMachine;

namespace DPlay.AICar.Car
{
    /// <summary>
    ///     Controls the car based on a custom algorithm to achieve avoidance behavior.
    /// </summary>
    public partial class CarControllerAvoidance
    {
        /// <summary>
        ///     FSM State for driving forwards.
        /// </summary>
        private class StateForward : FSM<CarControllerAvoidance>.IStateTo<StateBackwards>
        {
            /// <summary> The distance to a wall that is considered close. </summary>
            private const float CloseToWall = 2.5f;

            /// <summary> The multiplier for the linear speed input. </summary>
            private const float LinearInputMultiplier = 0.1f;

            /// <summary> The <see cref="CarControllerAvoidance"/> this state is for. </summary>
            public CarControllerAvoidance Self { get; set; }

            /// <summary>
            ///     Called every update while active. Sets the linear speed input.
            /// </summary>
            public void Update()
            {
                Self.linearSpeedInput = Self.forwardsDistance * LinearInputMultiplier;
            }

            /// <summary>
            ///     Indicates whether a transition to <see cref="StateBackwards"/> is allowed.
            /// </summary>
            /// <returns>A boolean indicating whether the transition may occur.</returns>
            bool FSM<CarControllerAvoidance>.IStateTo<StateBackwards>.MayTransition()
            {
                return Self.forwardsDistance < CloseToWall;
            }
        }
    }
}

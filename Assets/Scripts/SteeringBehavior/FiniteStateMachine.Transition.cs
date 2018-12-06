using System;

namespace DPlay.AICar.SteeringBehavior
{
    /// <summary>
    ///     Implements a simple Finite State Machine behavior with states and transitions.
    /// </summary>
    public sealed partial class FiniteStateMachine
    {
        /// <summary>
        ///     A class for representing a simple transition.
        /// </summary>
		public sealed class Transition
        {
            /// <summary> The state that has to active to trigger.</summary>
            public readonly IState FromState;

            /// <summary> The state that will become active on triggering. </summary>
            public readonly IState ToState;

            /// <summary> The condition that has to match. </summary>
            private readonly Func<bool> condition;

            /// <summary>
            ///     Initializes a new instance of the <see cref="Transition"/> class.
            /// </summary>
            /// <param name="from">The state that will be transitioned from.</param>
            /// <param name="to">The state that will be transitioned to.</param>
            /// <param name="condition">The condition that has to be fulfilled for the transition.</param>
            public Transition(IState from, IState to, Func<bool> condition)
            {
                FromState = from;
                ToState = to;

                this.condition = condition;
            }

            /// <summary>
            ///     Returns a boolean indicating whether or not the transition may occur.
            /// </summary>
            /// <returns>Whether or not to transition.</returns>
            public bool MayPerform()
            {
                return condition.Invoke();
            }
        }
    }
}

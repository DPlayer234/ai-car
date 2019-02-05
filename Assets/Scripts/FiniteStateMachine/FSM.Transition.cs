using System;

namespace DPlay.AICar.FiniteStateMachine
{
    /// <summary>
    ///     Implements a simple Finite State Machine behavior with states and transitions.
    /// </summary>
    /// <typeparam name="T">The type of the referrence object <seealso cref="Self"/></typeparam>
    public sealed partial class FSM<T>
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
            private readonly TransitionCondition condition;

            /// <summary>
            ///     Initializes a new instance of the <see cref="Transition"/> class.
            /// </summary>
            /// <param name="from">The state that will be transitioned from.</param>
            /// <param name="to">The state that will be transitioned to.</param>
            /// <param name="condition">The condition that has to be fulfilled for the transition.</param>
            public Transition(IState from, IState to, TransitionCondition condition)
            {
                FromState = from;
                ToState = to;

                this.condition = condition;
            }

            /// <summary>
            ///     Represents a function that indicates whether a transition may occur.
            /// </summary>
            /// <returns>A boolean indicating whether the transition may occur.</returns>
            public delegate bool TransitionCondition();

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

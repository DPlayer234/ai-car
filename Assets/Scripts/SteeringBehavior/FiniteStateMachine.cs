using System.Collections.Generic;

namespace DPlay.AICar.SteeringBehavior
{
    /// <summary>
    ///     Implements a simple Finite State Machine behavior with states and transitions.
    /// </summary>
    public sealed partial class FiniteStateMachine
    {
        /// <summary> Internal field to store the active state. Use <see cref="ActiveState"/> instead. </summary>
        private IState activeState;

        /// <summary> A list of all registered transitions. </summary>
        public List<Transition> Transitions = new List<Transition>();

        /// <summary>
        ///     The currently active and therefore updated state.
        ///     If set, calls <see cref="IState.Enter"/> and <see cref="IState.Exit"/> respectively.
        /// </summary>
        public IState ActiveState
        {
            get
            {
                return activeState;
            }

            set
            {
                if (activeState != null)
                {
                    activeState.Exit();
                }

                value.Enter();
                activeState = value;
            }
        }

        /// <summary>
        ///     Registers a transition from one state to another.
        ///     Make sure to implement <see cref="IState{T}"/>!
        /// </summary>
        /// <typeparam name="TFrom">The type of the state to transition from.</typeparam>
        /// <typeparam name="TTo">The type of the state to transition to.</typeparam>
        /// <param name="from">The state to transition from.</param>
        /// <param name="to">The state to transition to.</param>
        /// <returns>The new transition object.</returns>
        public Transition AddTransition<TFrom, TTo>(TFrom from, TTo to)
            where TFrom : IState<TTo>
            where TTo : IState
        {
            Transition trans = new Transition(from, to, from.MayTransition);
            Transitions.Add(trans);

            return trans;
        }

        /// <summary>
        ///     Updates the active state and checks for transitions.
        /// </summary>
        public void Update()
        {
            ActiveState?.Update();

            ApplyPossibleTransitions();
        }

        /// <summary>
        ///     Applies the first possible transition from the current state..
        /// </summary>
        private void ApplyPossibleTransitions()
        {
            for (int i = 0; i < Transitions.Count; i++)
            {
                Transition trans = Transitions[i];

                if (trans.FromState == ActiveState && trans.MayPerform())
                {
                    ActiveState = trans.ToState;
                    return;
                }
            }
        }
    }
}

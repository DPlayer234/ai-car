using System.Collections.Generic;

namespace DPlay.AICar.FiniteStateMachine
{
    /// <summary>
    ///     Implements a simple Finite State Machine behavior with states and transitions.
    /// </summary>
    /// <typeparam name="T">The type of the referrence object <seealso cref="Self"/></typeparam>
    public sealed partial class FSM<T>
    {
        /// <summary> A list of all registered transitions. </summary>
        public List<Transition> Transitions = new List<Transition>();

        /// <summary> Internal field to store the active state. Use <see cref="ActiveState"/> instead. </summary>
        private IState activeState;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FSM{T}"/> class.
        /// </summary>
        /// <param name="self">The referrence object.</param>
        public FSM(T self)
        {
            Self = self;
        }

        /// <summary> The referrence object. </summary>
        public T Self { get; private set; }

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
                (activeState as IStateEnterExit)?.Exit();

                if (value != null)
                {
                    value.Self = Self;
                }

                (value as IStateEnterExit)?.Enter();

                activeState = value;
            }
        }

        /// <summary>
        ///     Registers a transition from one state to another.
        ///     Make sure to implement <see cref="IStateTo{T}"/>!
        /// </summary>
        /// <typeparam name="TTo">The type of the state to transition to.</typeparam>
        /// <param name="from">The state to transition from.</param>
        /// <param name="to">The state to transition to.</param>
        /// <returns>The new transition object.</returns>
        public Transition AddTransition<TTo>(IStateTo<TTo> from, TTo to)
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

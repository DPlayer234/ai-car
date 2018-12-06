using System.Collections.Generic;

namespace DPlay.AICar.SteeringBehavior
{
    public sealed partial class FiniteStateMachine
    {
        private IState activeState;

        public List<IState> States = new List<IState>();

        public List<Transition> Transitions = new List<Transition>();

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

        public TState AddState<TState>(TState state) where TState : IState
        {
            States.Add(state);

            if (ActiveState == null)
            {
                ActiveState = state;
            }

            return state;
        }

        public Transition AddTransition<TFrom, TTo>(TFrom from, TTo to)
            where TFrom : IState<TTo>
            where TTo : IState
        {
            Transition trans = new Transition(from, to, from.CanTransition);
            Transitions.Add(trans);
            return trans;
        }

        public void Update()
        {
            ActiveState?.Update();
            
            for (int i = 0; i < Transitions.Count; i++)
            {
                Transition trans = Transitions[i];

                if (trans.FromState == ActiveState && trans.IsConditionFulfilled())
                {
                    ActiveState = trans.ToState;
                    return;
                }
            }
        }
    }
}

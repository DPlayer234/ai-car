using System;

namespace DPlay.AICar.SteeringBehavior
{
    public sealed partial class FiniteStateMachine
    {
		public sealed class Transition
        {
            public readonly IState FromState;

            public readonly IState ToState;

            private readonly Func<bool> condition;

            public Transition(IState from, IState to, Func<bool> condition)
            {
                FromState = from;
                ToState = to;

                this.condition = condition;
            }

            public bool IsConditionFulfilled()
            {
                return condition.Invoke();
            }
        }
    }
}

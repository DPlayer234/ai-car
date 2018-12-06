namespace DPlay.AICar.SteeringBehavior
{
    public sealed partial class FiniteStateMachine
    {
		public interface IState<T> : IState
            where T : IState
        {
            bool CanTransition();
        }
    }
}

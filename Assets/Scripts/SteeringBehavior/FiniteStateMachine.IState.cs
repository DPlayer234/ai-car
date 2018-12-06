namespace DPlay.AICar.SteeringBehavior
{
    public sealed partial class FiniteStateMachine
    {
		public interface IState
        {
            void Enter();

            void Exit();

            void Update();
        }
    }
}

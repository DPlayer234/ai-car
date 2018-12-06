namespace DPlay.AICar.SteeringBehavior
{
    /// <summary>
    ///     Implements a simple Finite State Machine behavior with states and transitions.
    /// </summary>
    public sealed partial class FiniteStateMachine
    {
        /// <summary>
        ///     Interface for any State.
        /// </summary>
		public interface IState
        {
            /// <summary>
            ///     Called when the state is entered/set as active.
            /// </summary>
            void Enter();

            /// <summary>
            ///     Called when the state is exitted/set as not active.
            /// </summary>
            void Exit();

            /// <summary>
            ///     Called in <see cref="FiniteStateMachine.Update"/> if it the active state.
            /// </summary>
            void Update();
        }
    }
}

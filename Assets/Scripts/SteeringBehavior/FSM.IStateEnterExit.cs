namespace DPlay.AICar.SteeringBehavior
{
    /// <summary>
    ///     Implements a simple Finite State Machine behavior with states and transitions.
    /// </summary>
    /// <typeparam name="T">The type of the referrence object <seealso cref="Self"/></typeparam>
    public sealed partial class FSM<T>
    {
        /// <summary>
        ///     Interface for any State with Enter and Exit callbacks.
        /// </summary>
		public interface IStateEnterExit : IState
        {
            /// <summary>
            ///     Called when the state is entered/set as active.
            /// </summary>
            void Enter();

            /// <summary>
            ///     Called when the state is exitted/set as not active.
            /// </summary>
            void Exit();
        }
    }
}

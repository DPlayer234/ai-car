namespace DPlay.AICar.FiniteStateMachine
{
    /// <summary>
    ///     Implements a simple Finite State Machine behavior with states and transitions.
    /// </summary>
    /// <typeparam name="T">The type of the referrence object <seealso cref="Self"/></typeparam>
    public sealed partial class FSM<T>
    {
        /// <summary>
        ///     Interface for any State that is able to transition to <typeparamref name="TTo"/>.
        ///     It is recommended to implement this interface explicitly when there are 2 or more possible transitions from this state.
        ///     It is also necessary to call <see cref="AddTransition{TTo}(IStateTo{TTo}, TTo)"/> to register the transition afterwards.
        /// </summary>
        /// <typeparam name="TTo">The type of the state to transition to.</typeparam>
		public interface IStateTo<TTo> : IState
            where TTo : IState
        {
            /// <summary>
            ///     Returns a boolean indicating whether or not the transition may occur.
            /// </summary>
            /// <returns>Whether or not to transition.</returns>
            bool MayTransition();
        }
    }
}

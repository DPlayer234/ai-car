namespace DPlay.AICar.SteeringBehavior
{
    /// <summary>
    ///     Implements a simple Finite State Machine behavior with states and transitions.
    /// </summary>
    public sealed partial class FiniteStateMachine
    {
        /// <summary>
        ///     Interface for any State that is able to transition to <typeparamref name="T"/>.
        ///     It is recommended to implement this interface explicitly when there are 2 or more possible transitions from this state.
        ///     It is also necessary to call <see cref="FiniteStateMachine.AddTransition{TTo}(IState{TTo}, TTo)"/> to register the transition afterwards.
        /// </summary>
        /// <typeparam name="T"></typeparam>
		public interface IState<T> : IState
            where T : IState
        {
            /// <summary>
            ///     Returns a boolean indicating whether or not the transition may occur.
            /// </summary>
            /// <returns>Whether or not to transition.</returns>
            bool MayTransition();
        }
    }
}

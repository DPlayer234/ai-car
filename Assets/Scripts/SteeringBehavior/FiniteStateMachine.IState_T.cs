namespace DPlay.AICar.SteeringBehavior
{
    /// <summary>
    ///     Implements a simple Finite State Machine behavior with states and transitions.
    /// </summary>
    public sealed partial class FiniteStateMachine
    {
        /// <summary>
        ///     Interface for any State that is able to transition to <typeparamref name="TTo"/>.
        /// </summary>
        /// <typeparam name="TTo"></typeparam>
		public interface IState<TTo> : IState
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

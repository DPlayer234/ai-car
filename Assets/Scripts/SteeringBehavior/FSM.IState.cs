namespace DPlay.AICar.SteeringBehavior
{
    /// <summary>
    ///     Implements a simple Finite State Machine behavior with states and transitions.
    /// </summary>
    /// <typeparam name="T">The type of the referrence object <seealso cref="Self"/></typeparam>
    public sealed partial class FSM<T>
    {
        /// <summary>
        ///     Interface for any State.
        /// </summary>
		public interface IState
        {
            /// <summary> The referrence object this state is for. </summary>
            T Self { get; set; }

            /// <summary>
            ///     Called in <see cref="FSM.Update"/> if it the active state.
            /// </summary>
            void Update();
        }
    }
}

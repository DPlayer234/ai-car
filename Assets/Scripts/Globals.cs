using System;

namespace DPlay.AICar
{
    /// <summary>
    ///     Contains global objects and variables.
    /// </summary>
    public static class Globals
    {
        /// <summary>
        ///     A global <see cref="System.Random"/> object for generating random numbers.
        /// </summary>
        public static readonly Random Random = new Random();

        /// <summary>
        ///     A number for a large distance.
        /// </summary>
        public const float LargeDistance = 6e6f;
    }
}

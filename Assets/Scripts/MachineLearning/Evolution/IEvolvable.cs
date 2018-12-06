using System;

namespace DPlay.AICar.MachineLearning.Evolution
{
    /// <summary>
    ///     Represents an evolvable <see cref="Component"/> for use with <see cref="EvolutionManager"/>.
    ///     The <seealso cref="IComparable{IEvolvable}"/> implementation is expected to sort by <seealso cref="Fitness"/>.
    /// </summary>
    public interface IEvolvable : IComparable<IEvolvable>
    {
        /// <summary> Is the Component enabled? (Refers to <see cref="Behaviour.enabled"/>) </summary>
        bool enabled { get; }

        /// <summary> The Fitness of the component, aka how "good" it is. </summary>
        double Fitness { get; }

        /// <summary>
        ///     Gets the current genome.
        /// </summary>
        /// <returns>An array of <seealso cref="double"/>s, representing the genome.</returns>
        double[] GetGenome();

        /// <summary>
        ///     Overrides the current genome.
        /// </summary>
        /// <param name="genes">An array of <seealso cref="double"/>s, representing the new genome.</param>
        void SetGenome(double[] genes);
    }
}

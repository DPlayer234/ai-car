using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DPlay.AICar.MachineLearning
{
    /// <summary>
    ///     Delegate type for <see cref="Neuron.ActivationFunction"/>s.
    /// </summary>
    /// <param name="sum">The Sum of all input <seealso cref="INeuron"/>s's Predicted Values.</param>
    /// <returns>The result of some calculation on the sum</returns>
    public delegate double ActivationFunction(double sum);

    /// <summary>
    ///     Static class to hold a couple of standard <see cref="ActivationFunction"/>s.
    /// </summary>
    public static class ActivationFunctions
    {
        /// <summary> Identity function: f(x) = x </summary>
        public static ActivationFunction Identity { get; } = (sum) => sum;

        /// <summary> Threshold at 0: f(x) = x > 0 ? 1 : -1 </summary>
        public static ActivationFunction Threshold0 { get; } = (sum) => sum > 0.0 ? 1.0 : -1.0;

        /// <summary> Tangens Hyperbolicus: f(x) = tanh(x) </summary>
        public static ActivationFunction Tanh { get; } = Math.Tanh;
    }
}

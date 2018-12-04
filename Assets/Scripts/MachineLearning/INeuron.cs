using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DPlay.AICar.MachineLearning
{
    /// <summary>
    ///     Interface for neurons and nerves.
    /// </summary>
    public interface INeuron
    {
        /// <summary> A list of input neurons. </summary>
        INeuron[] Inputs { get; }

        /// <summary> The weights for summing the input values. </summary>
        double[] Weights { get; }

        /// <summary> The bias, an additional value added to the sum. </summary>
        double Bias { get; set; }

        /// <summary> The last predicted value. </summary>
        double PredictedValue { get; }

        /// <summary>
        ///     Calculates a new predicted value.
        /// </summary>
        /// <returns>The new predicted value.</returns>
        double Predict();
    }
}

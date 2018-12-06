namespace DPlay.AICar.MachineLearning
{
    /// <summary>
    ///     A "Nerve" -- Implements the <see cref="INeuron"/> interface solely for inputting values into a system.
    ///     May set <see cref="PredictedValue"/> explicitly and has no own inputs.
    /// </summary>
    public class Nerve : INeuron
    {
        /// <summary> A zero-length array of <seealso cref="INeuron"/>s </summary>
        private static INeuron[] zeroNeurons = new INeuron[0];

        /// <summary> A zero-length array of <seealso cref="double"/>s </summary>
        private static double[] zeroDoubles = new double[0];

        /// <summary> Returns the inputs. Always a zero-length array. </summary>
        public INeuron[] Inputs => zeroNeurons;

        /// <summary> Returns the weights. Always a zero-length array. </summary>
        public double[] Weights => zeroDoubles;

        /// <summary> Returns the bias. Does not factor into the prediction. </summary>
        public double Bias { get; set; }

        /// <summary> The predicted value. To be explicitly set as an input. </summary>
        public double PredictedValue { get; set; }

        /// <summary>
        ///     Does no calculation and simply returns <see cref="PredictedValue"/>.
        /// </summary>
        /// <returns>The set predicted value.</returns>
        public double Predict() => PredictedValue;
    }
}

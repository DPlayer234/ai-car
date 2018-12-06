using System;

namespace DPlay.AICar.MachineLearning
{
    /// <summary>
    ///     A Neural Net for use in machine learning.
    /// </summary>
    public class NeuralNet
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NeuralNet"/> class in respect to the given input parameters.
        /// </summary>
        /// <param name="inputNerveCount">The amount of input nerves</param>
        /// <param name="layerNeuronCounts">The amount of neurons for each layer. Every additional value represents one layer.</param>
        /// <exception cref="ArgumentOutOfRangeException">No layers were specified</exception>
        public NeuralNet(int inputNerveCount, params int[] layerNeuronCounts)
        {
            if (layerNeuronCounts.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(layerNeuronCounts), "At least one layer has to exist for a NeuralNet.");
            }

            Inputs = new Nerve[inputNerveCount];

            for (int i = 0; i < inputNerveCount; i++)
            {
                Inputs[i] = new Nerve();
            }

            INeuron[] nextInputs = Inputs.CopyAs<Nerve, INeuron>();

            Layers = new NeuronLayer[layerNeuronCounts.Length];

            NeuronLayer lastLayer = null;

            for (int i = 0; i < layerNeuronCounts.Length; i++)
            {
                int neuronCount = layerNeuronCounts[i];

                lastLayer = new NeuronLayer(nextInputs, neuronCount);
                nextInputs = lastLayer.Outputs;

                Layers[i] = lastLayer;
            }

            Outputs = lastLayer.Outputs;
            OutputLayer = lastLayer;
            PredictedValues = new double[Outputs.Length];
        }

        /// <summary>
        ///     Input Nerves to the system. May be more easily set via <seealso cref="SetInputValues(double[])"/>.
        /// </summary>
        public Nerve[] Inputs { get; private set; }

        /// <summary>
        ///     Output Neurons of the system. It is best to call <seealso cref="Predict"/> first after changing the inputs.
        /// </summary>
        public INeuron[] Outputs { get; private set; }

        /// <summary>
        ///     All layers of the net, including the <seealso cref="OutputLayer"/>.
        /// </summary>
        public NeuronLayer[] Layers { get; private set; }

        /// <summary>
        ///     The last layer of the net containing the <seealso cref="Outputs"/>.
        /// </summary>
        public NeuronLayer OutputLayer { get; private set; }

        /// <summary>
        ///     The last set of predicted values of the <seealso cref="OutputLayer"/>.
        /// </summary>
        public double[] PredictedValues { get; private set; }

        /// <summary>
        ///     Sets the values of the <seealso cref="Inputs"/>.
        /// </summary>
        /// <param name="values">The values to be set.</param>
        public void SetInputValues(params double[] values)
        {
            if (values.Length != Inputs.Length)
            {
                throw new ArgumentException("Amount of input values does not match amount of Input Neurons!");
            }

            for (int i = 0; i < values.Length; i++)
            {
                Inputs[i].PredictedValue = values[i];
            }
        }

        /// <summary>
        ///     Predicts new values based on the current input values.
        /// </summary>
        /// <returns>The new set of predicted values of the <seealso cref="OutputLayer"/>.</returns>
        public double[] Predict()
        {
            for (int i = 0; i < Layers.Length; i++)
            {
                NeuronLayer layer = Layers[i];

                layer.Predict();
            }

            return PredictedValues = OutputLayer.PredictedValues;
        }

        /// <summary>
        ///     Predicts new values based on the given input values.
        ///     Internally just calls <see cref="SetInputValues(double[])"/> and then <see cref="Predict"/>.
        /// </summary>
        /// <returns>The new set of predicted values of the <seealso cref="OutputLayer"/>.</returns>
        public double[] Predict(params double[] values)
        {
            SetInputValues(values);
            return Predict();
        }

        /// <summary>
        ///     Gets all weights, including biases, of the net in a flat array.
        /// </summary>
        /// <returns>An array of all weights.</returns>
        public double[] GetAllWeights()
        {
            double[] weights = new double[GetWeightCount()];
            int weightIndex = 0;

            for (int index = 0; index < Layers.Length; index++)
            {
                NeuronLayer layer = Layers[index];

                for (int o = 0; o < layer.Outputs.Length; o++)
                {
                    INeuron output = layer.Outputs[o];

                    for (int w = 0; w < output.Weights.Length; w++)
                    {
                        weights[weightIndex++] = output.Weights[w];
                    }

                    weights[weightIndex++] = output.Bias;
                }
            }

            return weights;
        }

        /// <summary>
        ///     Sets all weights, including biases, of the net with a flat array.
        /// </summary>
        /// <param name="weights">An array of all weights.</param>
        /// <exception cref="ArgumentException">Amount of weights supplied does not match amount of weights expected.</exception>
        public void SetAllWeights(double[] weights)
        {
            if (weights.Length != GetWeightCount())
            {
                throw new ArgumentException("Amount of weights supplied does not match amount of weights expected.", nameof(weights));
            }

            int weightIndex = 0;

            for (int index = 0; index < Layers.Length; index++)
            {
                NeuronLayer layer = Layers[index];

                for (int o = 0; o < layer.Outputs.Length; o++)
                {
                    INeuron output = layer.Outputs[o];

                    for (int w = 0; w < output.Weights.Length; w++)
                    {
                        output.Weights[w] = weights[weightIndex++];
                    }

                    output.Bias = weights[weightIndex++];
                }
            }
        }

        /// <summary>
        ///     Gets the total amount of weights, including biases, in the system.
        /// </summary>
        /// <returns>The amount of weights and biases</returns>
        public int GetWeightCount()
        {
            int weightCount = 0;

            for (int i = 0; i < Layers.Length; i++)
            {
                weightCount += (Layers[i].Inputs.Length + 1) * Layers[i].Outputs.Length;
            }

            return weightCount;
        }

        /// <summary>
        ///     Sets the activation function of every single neuron in the net.
        /// </summary>
        /// <param name="activationFunction">The activation function to set.</param>
        public void SetAllActivationFunctions(ActivationFunction activationFunction)
        {
            for (int index = 0; index < Layers.Length; index++)
            {
                Layers[index].SetAllActivationFunctions(activationFunction);
            }
        }
    }
}

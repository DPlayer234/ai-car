using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DPlay.AICar.MachineLearning
{
    /// <summary>
    ///     A layer in a <see cref="NeuralNet"/>.
    /// </summary>
    public class NeuronLayer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NeuronLayer"/> class.
        ///     Uses the given input neurons and generates a new set of output neurons.
        /// </summary>
        /// <param name="inputs">The input neurons to use.</param>
        /// <param name="outputCount">The amount of output neurons to generate.</param>
        public NeuronLayer(INeuron[] inputs, int outputCount)
        {
            Inputs = inputs;

            Outputs = new Neuron[outputCount];

            for (int o = 0; o < Outputs.Length; o++)
            {
                Outputs[o] = new Neuron(inputs);
            }

            PredictedValues = new double[Outputs.Length];
        }

        /// <summary>
        ///     This layer's input neurons.
        /// </summary>
        public INeuron[] Inputs { get; private set; }

        /// <summary>
        ///     This layer's output neurons.
        /// </summary>
        public INeuron[] Outputs { get; private set; }

        /// <summary>
        ///     The last set of predicted values.
        /// </summary>
        public double[] PredictedValues { get; private set; }

        /// <summary>
        ///     Predicts new values based on the current input values.
        /// </summary>
        /// <returns>The new set of predicted values.</returns>
        public double[] Predict()
        {
            for (int i = 0; i < Outputs.Length; i++)
            {
                PredictedValues[i] = Outputs[i].Predict();
            }

            return PredictedValues;
        }

        /// <summary>
        ///     Sets the activation function of every neuron in the layer.
        /// </summary>
        /// <param name="activationFunction">The activation function to set.</param>
        public void SetAllActivationFunctions(ActivationFunction activationFunction)
        {
            for (int o = 0; o < Outputs.Length; o++)
            {
                Neuron output = Outputs[o] as Neuron;

                if (output != null)
                {
                    output.ActivationFunction = activationFunction;
                }
            }
        }
    }
}

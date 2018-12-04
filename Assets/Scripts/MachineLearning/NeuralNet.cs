using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPlay.AICar.MachineLearning
{
    public class NeuralNet
    {
        public NeuralNet(int inputNeuronCount, params int[] layerNeuronCounts)
        {
            if (layerNeuronCounts.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(layerNeuronCounts), "At least one layer has to exist for a NeuralNet.");
            }

            Inputs = new Nerve[inputNeuronCount];

            for (int i = 0; i < inputNeuronCount; i++)
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

        public Nerve[] Inputs { get; private set; }

        public INeuron[] Outputs { get; private set; }

        public NeuronLayer[] Layers { get; private set; }

        public NeuronLayer OutputLayer { get; private set; }

        public double[] PredictedValues { get; private set; }

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

        public double[] Predict()
        {
            for (int i = 0; i < Layers.Length; i++)
            {
                NeuronLayer layer = Layers[i];

                layer.Predict();
            }

            return PredictedValues = OutputLayer.PredictedValues;
        }

        public double[] Predict(params double[] values)
        {
            SetInputValues(values);
            return Predict();
        }

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

        public int GetWeightCount()
        {
            int weightCount = 0;

            for (int i = 0; i < Layers.Length; i++)
            {
                weightCount += (Layers[i].Inputs.Length + 1) * Layers[i].Outputs.Length;
            }

            return weightCount;
        }

        public void SetAllActivationFunctions(ActivationFunction activationFunction)
        {
            for (int index = 0; index < Layers.Length; index++)
            {
                Layers[index].SetAllActivationFunctions(activationFunction);
            }
        }
    }
}

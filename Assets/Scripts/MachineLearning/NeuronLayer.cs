using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DPlay.AICar.MachineLearning
{
    public class NeuronLayer
    {
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

        public INeuron[] Inputs { get; private set; }

        public INeuron[] Outputs { get; private set; }

        public double[] PredictedValues { get; private set; }

        public double[] Predict()
        {
            for (int i = 0; i < Outputs.Length; i++)
            {
                PredictedValues[i] = Outputs[i].Predict();
            }

            return PredictedValues;
        }

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

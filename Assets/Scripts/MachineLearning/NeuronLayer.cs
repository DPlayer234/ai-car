using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public INeuron[] Inputs { get; private set; }

        public INeuron[] Outputs { get; private set; }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPlay.AICar.MachineLearning
{
    public class Nerve : INeuron
    {
        private static INeuron[] zeroNeurons = new INeuron[0];

        private static double[] zeroDoubles = new double[0];

        public INeuron[] Inputs => zeroNeurons;

        public double[] Weights => zeroDoubles;

        public double Bias { get; set; }

        public double PredictedValue { get; set; }

        public double Predict() => PredictedValue;
    }
}

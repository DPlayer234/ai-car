using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DPlay.AICar.MachineLearning
{
    public interface INeuron
    {
        INeuron[] Inputs { get; }

        double[] Weights { get; }

        double Bias { get; set; }

        double Predict();
    }
}

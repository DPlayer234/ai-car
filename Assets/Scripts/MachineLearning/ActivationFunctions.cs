using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DPlay.AICar.MachineLearning
{
    public delegate double ActivationFunction(double sum);

    public static class ActivationFunctions
    {
        public static ActivationFunction Linear { get; } = (sum) => sum;

        public static ActivationFunction Threshold0 { get; } = (sum) => sum > 0.0 ? 1.0 : -1.0;

        public static ActivationFunction Tanh { get; } = Math.Tanh;
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DPlay.AICar.MachineLearning
{
    public class Neuron : INeuron
    {
        public ActivationFunction ActivationFunction = ActivationFunctions.Linear;

        private double[] weights;

        private INeuron[] inputs;

        private static Random random = new Random();

        public Neuron(INeuron[] inputs)
        {
            this.inputs = inputs;

            Weights = new double[inputs.Length];

            // Randomly initialize all weights and the bias
            for (int i = 0; i < inputs.Length; i++)
            {
                Weights[i] = GetRandomStartingValue();
            }

            Bias = GetRandomStartingValue();
        }

        public INeuron[] Inputs
        {
            get
            {
                return inputs;
            }

            set
            {
                AssertInputCount(value.Length);

                inputs = value;
            }
        }

        public double[] Weights
        {
            get
            {
                return weights;
            }

            set
            {
                AssertInputCount(value.Length);

                weights = value.CopyArray();
            }
        }

        public double Bias { get; set; }

        public double Predict()
        {
            return ActivationFunction(SumInputValues());
        }

        /// <summary>
        ///     Returns a random Value [-1.0 .. 1.0).
        /// </summary>
        /// <returns>A random value</returns>
        private static double GetRandomStartingValue()
        {
            return random.NextDouble() * 2.0 - 1.0;
        }

        private double SumInputValues()
        {
            double sum = Bias;

            for (int i = 0; i < Inputs.Length; i++)
            {
                sum += Inputs[i].Predict() * Weights[i];
            }

            return sum;
        }

        private void AssertInputCount(int count)
        {
            if (count != Inputs.Length)
            {
                throw new ArgumentException("Amount of Input Neurons does not match InputCount!");
            }
        }
    }
}
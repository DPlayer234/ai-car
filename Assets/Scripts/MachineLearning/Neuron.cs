using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using Random = System.Random;

namespace DPlay.AICar.MachineLearning
{
    /// <summary>
    ///     Implements the <see cref="INeuron"/> interface.
    ///     Represents a basic neuron for a neural net.
    /// </summary>
    public class Neuron : INeuron
    {
        /// <summary>
        ///     The activation function which modifies the prediction.
        /// </summary>
        public ActivationFunction ActivationFunction = ActivationFunctions.Identity;

        /// <summary> Internally stores the inputs. (Use <seealso cref="Inputs"/> instead.) </summary>
        private INeuron[] inputs;

        /// <summary> Internally stores the weights. (Use <seealso cref="Weights"/> instead.) </summary>
        private double[] weights;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Neuron"/> class.
        ///     The amount of initial inputs dictates how many inputs it may ever have.
        /// </summary>
        /// <param name="inputs">The initial inputs.</param>
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

        /// <summary>
        ///     The inputs of the neuron. May be changed at any point, in any way.
        ///     Overriding only works with arrays of the same length.
        /// </summary>
        /// <exception cref="ArgumentException">New array does not have the same length.</exception>
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

        /// <summary>
        ///     The weights of the neuron inputs. May be changed at any point, in any way.
        ///     Overriding only works with arrays of the same length.
        /// </summary>
        /// <exception cref="ArgumentException">New array does not have the same length.</exception>
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

        /// <summary>
        ///     The bias, an additional value added to the sum. 
        /// </summary>
        public double Bias { get; set; }

        /// <summary>
        ///     The last predicted value.
        /// </summary>
        public double PredictedValue { get; private set; }

        /// <summary>
        ///     Calculates a new predicted value by summing the input values.
        /// </summary>
        /// <returns>The new predicated value.</returns>
        public double Predict()
        {
            return PredictedValue = ActivationFunction(SumInputValues());
        }

        /// <summary>
        ///     Returns a random Value [-1.0..1.0).
        /// </summary>
        /// <returns>A random value</returns>
        private static double GetRandomStartingValue()
        {
            return Globals.Random.NextDouble() * 2.0 - 1.0;
        }

        /// <summary>
        ///     Sums all input values multiplied by their weights and the bias.
        /// </summary>
        /// <returns>The sum.</returns>
        private double SumInputValues()
        {
            double sum = Bias;

            for (int i = 0; i < Inputs.Length; i++)
            {
                sum += Inputs[i].PredictedValue * Weights[i];
            }

            return sum;
        }

        /// <summary>
        ///     Throws an exception if <paramref name="count"/> is not the same as <see cref="Inputs"/>.Length.
        /// </summary>
        /// <param name="count">The count to assert.</param>
        /// <exception cref="ArgumentException"><paramref name="count"/> does not match Inputs.Length</exception>
        private void AssertInputCount(int count)
        {
            if (count != Inputs.Length)
            {
                throw new ArgumentException("Amount of elements does not match Inputs.Length!");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPlay.AICar.MachineLearning;
using DPlay.AICar.MachineLearning.Evolution;
using UnityEngine;

namespace DPlay.AICar.Car
{
    /// <summary>
    ///     A class based on <see cref="EvolutionManager"/> specifically to handle the evolution of <see cref="CarControllerNeural"/>.
    /// </summary>
    public class CarEvolutionManager : EvolutionManager
    {
        /// <summary> (Optional) <seealso cref="Car.CameraController"/> to follow the best current car. </summary>
        public CameraController CameraController;

        /// <summary>
        ///     Initializes additional fields of new generations' members.
        ///     Randomly sets the color of a possible <seealso cref="MaterialPainter"/>.
        /// </summary>
        /// <param name="gameObject">The <seealso cref="GameObject"/> to initialize.</param>
        protected override void InitializeChild(GameObject gameObject)
        {
            
            MaterialPainter painter = gameObject.GetComponent<MaterialPainter>();

            if (painter != null)
            {
                painter.Color = HelperFunctions.GetRandomColor();
            }
        }

        /// <summary>
        ///     Called by Unity to update the <seealso cref="CarEvolutionManager"/> each frame.
        /// </summary>
        protected override void Update()
        {
            if (CameraController != null)
            {
                double bestFitness = 0.0;
                CarController bestCar = null;

                foreach (var member in CurrentGeneration)
                {
                    if (member.enabled && member.Fitness > bestFitness)
                    {
                        bestFitness = member.Fitness;
                        bestCar = member as CarController;
                    }
                }

                if (bestCar != null)
                {
                    CameraController.ToFollow = bestCar.transform;
                }
            }

            base.Update();
        }
    }
}

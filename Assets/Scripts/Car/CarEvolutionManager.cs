using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPlay.AICar.MachineLearning.Evolution;
using UnityEngine;

namespace DPlay.AICar.Car
{
    public class CarEvolutionManager : EvolutionManager
    {
        /// <summary> (Optional) <seealso cref="Car.CameraController"/> to follow the best current car. </summary>
        public CameraController CameraController;

        protected override void InitializeChild(GameObject gameObject)
        {
            byte[] colorBytes = new byte[3];
            
            MaterialPainter painter = gameObject.GetComponent<MaterialPainter>();

            if (painter != null)
            {
                Globals.Random.NextBytes(colorBytes);

                painter.Color = new Color(
                    colorBytes[0] / 255.0f,
                    colorBytes[1] / 255.0f,
                    colorBytes[2] / 255.0f);
            }

            CarControllerNeural carController = gameObject.GetComponent<CarControllerNeural>();

            if (carController != null)
            {
                carController.DisableOnCrash = true;
                carController.DrawRayCastGizmos = false;
            }
        }

        protected override void Update()
        {
            if (CameraController != null)
            {
                double bestFitness = 0.0;
                CarController bestCar = null;

                foreach (var member in CurrentGeneration)
                {
                    if (member.Active && member.Fitness > bestFitness)
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

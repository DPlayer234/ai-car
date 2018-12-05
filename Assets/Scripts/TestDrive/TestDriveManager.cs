using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DPlay.AICar.Car;
using DPlay.AICar.MachineLearning;

namespace DPlay.AICar.TestDrive
{
    /// <summary>
    ///     Manages test driving
    /// </summary>
    public class TestDriveManager : MonoBehaviour
    {
        /// <summary> The prefab for cars with a neural network. </summary>
        public CarControllerNeural CarNeuralPrefab;

        /// <summary> The "spawn point" of the cars. Position and rotation of new members is overridden to the one of this <seealso cref="Transform"/>. </summary>
        public Transform SpawnPoint;

        /// <summary> How far from the <seealso cref="SpawnPoint"/> cars may be spawned. </summary>
        public float MaximumSpawnOffset;

        /// <summary> Root transform for all spawned cars. </summary>
        private Transform spawnRoot;

        /// <summary>
        ///     Destroys all spawned cars.
        /// </summary>
        public void DestroyAllCars()
        {
            if (spawnRoot != null)
            {
                Destroy(spawnRoot.gameObject);
            }

            spawnRoot = new GameObject("CarsRoot").transform;
            spawnRoot.parent = transform;
        }

        /// <summary>
        ///     Spawns a neural car with the given genome code.
        /// </summary>
        /// <param name="genomeCode">The genome code for the car's brain.</param>
        public void SpawnNeuralCar(string genomeCode)
        {
            SpawnNeuralCar(NeuralNetWeightSerializer.ToWeights(genomeCode));
        }

        /// <summary>
        ///     Spawns a neuron car with the given genes.
        /// </summary>
        /// <param name="genes">The genes for car's brain.</param>
        public void SpawnNeuralCar(double[] genes)
        {
            CarControllerNeural car = SpawnCar(CarNeuralPrefab);

            try
            {
                car.SetGenome(genes);
            }
            catch (ArgumentException)
            {
                Debug.LogError("The genome is invalid.");
                Destroy(car.gameObject);
            }
        }

        /// <summary>
        ///     Spawns a car using steering behavior.
        /// </summary>
        public void SpawnSteeringCar()
        {
            // TODO
        }

        /// <summary>
        ///     Spawns a car of a specific type and returns it.
        /// </summary>
        /// <typeparam name="TCar">The type of the <seealso cref="CarController"/></typeparam>
        /// <param name="prefab">The prefab used to spawn the new car.</param>
        /// <returns>The newly spawned <seealso cref="CarController"/></returns>
        private TCar SpawnCar<TCar>(TCar prefab) where TCar : CarController
        {
            TCar car = Instantiate(prefab, SpawnPoint.position, SpawnPoint.rotation, spawnRoot);

            car.transform.position += new Vector3(
                HelperFunctions.GetRandomFloat(-MaximumSpawnOffset, MaximumSpawnOffset),
                0.0f,
                HelperFunctions.GetRandomFloat(-MaximumSpawnOffset, MaximumSpawnOffset));

            MaterialPainter painter = car.GetComponent<MaterialPainter>();

            if (painter != null)
            {
                painter.Color = HelperFunctions.GetRandomColor();
            }

            return car;
        }

        /// <summary>
        ///     Called by Unity once to initialize the <see cref="TestDriveManager"/>.
        /// </summary>
        private void Awake()
        {
            DestroyAllCars();
        }
    }
}

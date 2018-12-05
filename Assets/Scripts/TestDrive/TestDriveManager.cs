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
    public class TestDriveManager : MonoBehaviour
    {
        public CarControllerNeural CarNeuralPrefab;

        /// <summary> The "spawn point" of the cars. Position and rotation of new members is overridden to the one of this <seealso cref="Transform"/>. </summary>
        public Transform SpawnPoint;

        public float MaximumSpawnOffset;

        private Transform spawnRoot;

        public void DestroyAllCars()
        {
            if (spawnRoot != null)
            {
                Destroy(spawnRoot.gameObject);
            }

            spawnRoot = new GameObject("CarsRoot").transform;
            spawnRoot.parent = transform;
        }

        public void SpawnNeuralCar(string genomeCode)
        {
            SpawnNeuralCar(NeuralNetWeightSerializer.ToWeights(genomeCode));
        }

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

        public void SpawnSteeringCar()
        {
            // TODO
        }

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

        private void Awake()
        {
            DestroyAllCars();
        }
    }
}

using System;
using System.Collections.Generic;
using DPlay.AICar.Car;
using DPlay.AICar.MachineLearning;
using DPlay.AICar.Misc;
using UnityEngine;

namespace DPlay.AICar.TestDrive
{
    /// <summary>
    ///     Manages test driving
    /// </summary>
    public class TestDriveManager : MonoBehaviour
    {
        /// <summary> <seealso cref="Car.CameraController"/> to follow a car. </summary>
        public CameraController CameraController;

        /// <summary> The prefab for cars with a neural network. </summary>
        public CarControllerNeural CarNeuralPrefab;

        /// <summary> The prefab for cars with steering behavior. </summary>
        public CarControllerSteering CarSteeringPrefab;

        /// <summary> The "spawn point" of the cars. Position and rotation of new members is overridden to the one of this <seealso cref="Transform"/>. </summary>
        public Transform SpawnPoint;

        /// <summary> How far from the <seealso cref="SpawnPoint"/> cars may be spawned. </summary>
        public float MaximumSpawnOffset;

        /// <summary> The default transform for <see cref="CameraController"/> to follow if no other exists. </summary>
        private Transform defaultCameraToFollow;

        /// <summary> Index of the transform in <see cref="cameraFollowTransforms"/> <see cref="CameraController"/> is following. </summary>
        private int currentFollowIndex = -1;

        /// <summary> Possible transforms for <see cref="CameraController"/> to follow </summary>
        private List<Transform> cameraFollowTransforms = new List<Transform>();

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
        
            cameraFollowTransforms.Clear();
            SwitchCameraFollow();
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
                return;
            }

            cameraFollowTransforms.Add(car.transform);
        }

        /// <summary>
        ///     Spawns a car using steering behavior.
        /// </summary>
        public void SpawnSteeringCar()
        {
            CarControllerSteering car = SpawnCar(CarSteeringPrefab);

            cameraFollowTransforms.Add(car.transform);
        }

        /// <summary>
        ///     Switches the car/transform the camera is following to the next one in line.
        /// </summary>
        public void SwitchCameraFollow()
        {
            currentFollowIndex += 1;

            if (currentFollowIndex >= cameraFollowTransforms.Count)
            {
                currentFollowIndex = -1;
            }

            if (currentFollowIndex < 0)
            {
                CameraController.ToFollow = defaultCameraToFollow;
            }
            else
            {
                CameraController.ToFollow = cameraFollowTransforms[currentFollowIndex];
            }
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
                painter.Color *= HelperFunctions.GetRandomColor();
            }
            
            return car;
        }

        /// <summary>
        ///     Called by Unity once to initialize the <see cref="TestDriveManager"/>.
        /// </summary>
        /// <exception cref="UnassignedReferenceException">No CameraController was assigned.</exception>
        private void Awake()
        {
            if (CameraController == null)
            {
                throw new UnassignedReferenceException("The CameraController was not assigned.");
            }

            defaultCameraToFollow = CameraController.ToFollow;

            DestroyAllCars();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPlay.AICar.Car;
using DPlay.AICar.MachineLearning;
using DPlay.AICar.TestDrive;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace DPlay.AICar.UIControls
{
    public class UITestDrive : MonoBehaviour
    {
        public TestDriveManager TestDriveManager;

        public GameObject NeuralAgentSpawnerElement;

        public GameObject SteeringAgentSpawnerElement;

        public GameObject ResetElement;

        public GameObject TopDownCameraElement;

        private void InitializeNeuralAgentSpawnerElement()
        {
            InputField inputField = NeuralAgentSpawnerElement.GetComponentInChildren<InputField>();
            Button spawnButton = NeuralAgentSpawnerElement.GetComponentInChildren<Button>();

            spawnButton.onClick.AddListener(() =>
            {
                double[] genes = NeuralNetWeightSerializer.ToWeights(inputField.text);
                TestDriveManager.SpawnNeuralCar(genes);
            });
        }

        private void InitializeSteeringAgentSpawnerElement()
        {
            Button spawnButton = SteeringAgentSpawnerElement.GetComponentInChildren<Button>();

            spawnButton.onClick.AddListener(() => TestDriveManager.SpawnSteeringCar());
        }

        private void InitializeResetElement()
        {
            Button spawnButton = ResetElement.GetComponentInChildren<Button>();

            spawnButton.onClick.AddListener(() => TestDriveManager.DestroyAllCars());
        }

        private void InitializeTopDownCameraElement()
        {
            Camera[] cameras = FindObjectsOfType<Camera>();
            if (cameras.Length != 2) return;

            Button swapButton = TopDownCameraElement.GetComponentInChildren<Button>();

            swapButton.onClick.AddListener(() =>
            {
                RenderTexture texture0 = cameras[0].targetTexture;
                RenderTexture texture1 = cameras[1].targetTexture;

                cameras[0].targetTexture = texture1;
                cameras[1].targetTexture = texture0;
            });
        }

        private void Awake()
        {
            if (NeuralAgentSpawnerElement != null)
            {
                InitializeNeuralAgentSpawnerElement();
            }

            if (SteeringAgentSpawnerElement != null)
            {
                InitializeSteeringAgentSpawnerElement();
            }

            if (ResetElement != null)
            {
                InitializeResetElement();
            }

            if (TopDownCameraElement != null)
            {
                InitializeTopDownCameraElement();
            }
        }
    }
}

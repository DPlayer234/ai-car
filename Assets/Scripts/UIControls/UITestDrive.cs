using DPlay.AICar.MachineLearning;
using DPlay.AICar.TestDrive;
using UnityEngine;
using UnityEngine.UI;

namespace DPlay.AICar.UIControls
{
    /// <summary>
    ///     Provides functions to be used in the Test Drive Menu and sets it up.
    /// </summary>
    [DisallowMultipleComponent]
    public class UITestDrive : MonoBehaviour
    {
        /// <summary> The used <seealso cref="TestDrive.TestDriveManager"/>. </summary>
        public TestDriveManager TestDriveManager;

        /// <summary> The root element for the neural agent spawner input field and button. </summary>
        public GameObject NeuralAgentSpawnerElement;

        /// <summary> The root element for the steering agent spawner button. </summary>
        public GameObject SteeringAgentSpawnerElement;

        /// <summary> The root element for the avoidance agent spawner button. </summary>
        public GameObject AvoidanceAgentSpawnerElement;

        /// <summary> The root element for the reset button. </summary>
        public GameObject ResetElement;

        /// <summary> The root element for the switch follow button. </summary>
        public GameObject SwitchFollowElement;

        /// <summary> The root element for the camera swap button. </summary>
        public GameObject TopDownCameraElement;

        /// <summary>
        ///     Initializes <see cref="NeuralAgentSpawnerElement"/>.
        /// </summary>
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

        /// <summary>
        ///     Initializes <see cref="SteeringAgentSpawnerElement"/>.
        /// </summary>
        private void InitializeSteeringAgentSpawnerElement()
        {
            Button spawnButton = SteeringAgentSpawnerElement.GetComponentInChildren<Button>();

            spawnButton.onClick.AddListener(() => TestDriveManager.SpawnSteeringCar());
        }

        /// <summary>
        ///     Initializes <see cref="AvoidanceAgentSpawnerElement"/>.
        /// </summary>
        private void InitializeAvoidanceAgentSpawnerElement()
        {
            Button spawnButton = AvoidanceAgentSpawnerElement.GetComponentInChildren<Button>();

            spawnButton.onClick.AddListener(() => TestDriveManager.SpawnAvoidanceCar());
        }

        /// <summary>
        ///     Initializes <see cref="ResetElement"/>.
        /// </summary>
        private void InitializeResetElement()
        {
            Button spawnButton = ResetElement.GetComponentInChildren<Button>();

            spawnButton.onClick.AddListener(() => TestDriveManager.DestroyAllCars());
        }

        /// <summary>
        ///     Initializes <see cref="SwitchFollowElement"/>.
        /// </summary>
        private void InitializeSwitchFollowElement()
        {
            Button spawnButton = SwitchFollowElement.GetComponentInChildren<Button>();

            spawnButton.onClick.AddListener(() => TestDriveManager.SwitchCameraFollow());
        }

        /// <summary>
        ///     Initializes <see cref="TopDownCameraElement"/>.
        /// </summary>
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

        /// <summary>
        ///     Called by Unity to initialize the <see cref="UITestDrive"/>.
        /// </summary>
        private void Awake()
        {
            if (TestDriveManager == null)
            {
                Debug.LogError("No TestDriveManager was assigned.");
                Destroy(this);
                return;
            }

            if (NeuralAgentSpawnerElement != null)
            {
                InitializeNeuralAgentSpawnerElement();
            }

            if (SteeringAgentSpawnerElement != null)
            {
                InitializeSteeringAgentSpawnerElement();
            }

            if (AvoidanceAgentSpawnerElement != null)
            {
                InitializeAvoidanceAgentSpawnerElement();
            }

            if (ResetElement != null)
            {
                InitializeResetElement();
            }

            if (SwitchFollowElement != null)
            {
                InitializeSwitchFollowElement();
            }

            if (TopDownCameraElement != null)
            {
                InitializeTopDownCameraElement();
            }
        }
    }
}

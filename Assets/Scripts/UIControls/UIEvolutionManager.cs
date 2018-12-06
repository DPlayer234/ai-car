using DPlay.AICar.MachineLearning.Evolution;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DPlay.AICar.UIControls
{
    /// <summary>
    ///     Provides function to be used in the Evolution Menu and sets it up.
    /// </summary>
    [DisallowMultipleComponent]
    public class UIEvolutionManager : MonoBehaviour
    {
        /// <summary> The used <seealso cref="MachineLearning.Evolution.EvolutionManager"/>. </summary>
        public EvolutionManager EvolutionManager;

        /// <summary> The root element for the maximum generation age slider. </summary>
        public GameObject MaximumGenerationAgeElement;

        /// <summary> The root element for the total car count slider. </summary>
        public GameObject TotalCountCarElement;

        /// <summary> The amount of random cars to spawn. </summary>
        [Range(0, 20)]
        public int RandomCarCount = 20;

        /// <summary> The root element for the current generation age display. </summary>
        public GameObject CurrentGenerationAgeElement;

        /// <summary> The root element for the current generation display. </summary>
        public GameObject CurrentGenerationElement;

        /// <summary> The root element for the evolution reset button. </summary>
        public GameObject EvolutionResetElement;

        /// <summary> The root element for the best genomes input fields. </summary>
        public GameObject BestGenomesElement;

        /// <summary> UI Text to write the current generation age into. </summary>
        private Text currentAgeText;

        /// <summary> UI Text to write the current generation into. </summary>
        private Text currentGenerationText;

        /// <summary> UI InputFields to write the best genomes into. </summary>
        private InputField[] bestGenomesFields;

        /// <summary> The index of the last generation whose genomes were written to <seealso cref="bestGenomesFields"/> to prevent duplicate calculations. </summary>
        private int lastGenerationIndex;

        /// <summary>
        ///     Initializes <see cref="MaximumGenerationAgeElement"/>.
        /// </summary>
        private void InitializeMaximumGenerateAgeElement()
        {
            Text valueText = MaximumGenerationAgeElement.GetComponentInChildren<Text>();
            Slider slider = MaximumGenerationAgeElement.GetComponentInChildren<Slider>();

            UnityAction<float> func = value =>
            {
                EvolutionManager.MaximumGenerationAge = value;
                valueText.text = value.ToString();
            };

            slider.onValueChanged.AddListener(func);
            func(slider.value);
        }

        /// <summary>
        ///     Initializes <see cref="TotalCountCarElement"/>.
        /// </summary>
        private void InitializeTotalCarCountElement()
        {
            Text valueText = TotalCountCarElement.GetComponentInChildren<Text>();
            Slider slider = TotalCountCarElement.GetComponentInChildren<Slider>();

            UnityAction<float> func = fvalue =>
            {
                int value = Mathf.RoundToInt(fvalue);
                EvolutionManager.TotalCount = value;
                EvolutionManager.ChildCount = value - RandomCarCount;
                valueText.text = value.ToString();
            };

            slider.onValueChanged.AddListener(func);
            func(slider.value);
        }

        /// <summary>
        ///     Initializes <see cref="CurrentGenerationAgeElement"/>.
        /// </summary>
        private void InitializeCurrentGenerationAgeElement()
        {
            currentAgeText = CurrentGenerationAgeElement.GetComponentInChildren<Text>();
        }

        /// <summary>
        ///     Initializes <see cref="CurrentGenerationElement"/>.
        /// </summary>
        private void InitializeCurrentGenerationElement()
        {
            currentGenerationText = CurrentGenerationElement.GetComponentInChildren<Text>();
        }

        /// <summary>
        ///     Initializes <see cref="EvolutionResetElement"/>.
        /// </summary>
        private void InitializeEvolutionResetElement()
        {
            Button button = EvolutionResetElement.GetComponentInChildren<Button>();

            button.onClick.AddListener(() =>
            {
                EvolutionManager.GenerateFirstGeneration();
            });
        }

        /// <summary>
        ///     Initializes <see cref="BestGenomesElement"/>.
        /// </summary>
        private void InitializeBestGenomesElement()
        {
            bestGenomesFields = BestGenomesElement.GetComponentsInChildren<InputField>();
        }

        /// <summary>
        ///     Called by Unity to update the <see cref="UIEvolutionManager"/> once per frame.
        ///     Updates texts in UI elements.
        /// </summary>
        private void Update()
        {
            if (currentAgeText != null)
            {
                currentAgeText.text = EvolutionManager.CurrentGenerationAge.ToString("f3");
            }

            if (currentGenerationText != null)
            {
                currentGenerationText.text = EvolutionManager.GenerationIndex.ToString();
            }

            if (bestGenomesFields != null && lastGenerationIndex != EvolutionManager.GenerationIndex)
            {
                string[] bestGenomeCodes = EvolutionManager.GetBestLastGenomeCodes();

                for (int i = 0; i < Mathf.Min(bestGenomesFields.Length, bestGenomeCodes.Length); i++)
                {
                    bestGenomesFields[i].text = bestGenomeCodes[i];
                }

                lastGenerationIndex = EvolutionManager.GenerationIndex;
            }
        }

        /// <summary>
        ///     Called by Unity to initialize the <see cref="UIEvolutionManager"/>.
        /// </summary>
        private void Awake()
        {
            if (EvolutionManager == null)
            {
                Debug.LogError("No EvolutionManager was assigned.");
                Destroy(this);
                return;
            }

            if (MaximumGenerationAgeElement != null)
            {
                InitializeMaximumGenerateAgeElement();
            }

            if (TotalCountCarElement != null)
            {
                InitializeTotalCarCountElement();
            }

            if (CurrentGenerationAgeElement != null)
            {
                InitializeCurrentGenerationAgeElement();
            }

            if (CurrentGenerationElement != null)
            {
                InitializeCurrentGenerationElement();
            }

            if (EvolutionResetElement != null)
            {
                InitializeEvolutionResetElement();
            }

            if (BestGenomesElement != null)
            {
                InitializeBestGenomesElement();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPlay.AICar.MachineLearning.Evolution;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace DPlay.AICar.UIControls
{
    public class UIEvolutionManager : MonoBehaviour
    {
        public EvolutionManager CarEvolutionManager;

        public GameObject MaximumGenerationAgeElement;

        public GameObject CurrentGenerationAgeElement;

        public GameObject CurrentGenerationElement;

        public GameObject EvolutionResetElement;

        public GameObject BestGenomesElement;

        private Text currentAgeText;

        private Text currentGenerationText;

        private InputField[] bestGenomesFields;

        private int lastGenerationIndex;

        private void InitializeMaximumGenerateAgeElement()
        {
            Text valueText = MaximumGenerationAgeElement.GetComponentInChildren<Text>();
            Slider slider = MaximumGenerationAgeElement.GetComponentInChildren<Slider>();

            UnityAction<float> func = value =>
            {
                CarEvolutionManager.MaximumGenerationAge = value;
                valueText.text = value.ToString();
            };

            slider.onValueChanged.AddListener(func);
            func(slider.value);
        }

        private void InitializeCurrentGenerationAgeElement()
        {
            currentAgeText = CurrentGenerationAgeElement.GetComponentInChildren<Text>();
        }

        private void InitializeCurrentGenerationElement()
        {
            currentGenerationText = CurrentGenerationElement.GetComponentInChildren<Text>();
        }

        private void InitializeEvolutionResetElement()
        {
            Button button = EvolutionResetElement.GetComponentInChildren<Button>();

            button.onClick.AddListener(() =>
            {
                CarEvolutionManager.GenerateFirstGeneration();
            });
        }

        private void InitializeBestGenomesElement()
        {
            bestGenomesFields = BestGenomesElement.GetComponentsInChildren<InputField>();
        }

        private void Update()
        {
            if (currentAgeText != null)
            {
                currentAgeText.text = CarEvolutionManager.CurrentGenerationAge.ToString("f3");
            }

            if (currentGenerationText != null)
            {
                currentGenerationText.text = CarEvolutionManager.GenerationIndex.ToString();
            }

            if (bestGenomesFields != null && lastGenerationIndex != CarEvolutionManager.GenerationIndex)
            {
                string[] bestGenomeCodes = CarEvolutionManager.GetBestLastGenomeCodes();

                for (int i = 0; i < Mathf.Min(bestGenomesFields.Length, bestGenomeCodes.Length); i++)
                {
                    bestGenomesFields[i].text = bestGenomeCodes[i];
                }

                lastGenerationIndex = CarEvolutionManager.GenerationIndex;
            }
        }

        private void Awake()
        {
            if (MaximumGenerationAgeElement != null)
            {
                InitializeMaximumGenerateAgeElement();
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

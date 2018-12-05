using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DPlay.AICar.UIControls
{
    /// <summary>
    ///     Scales a UI Element in such a way, that it's ratio stays constant.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UIRatioScalar : MonoBehaviour
    {
        /// <summary> The ratio for scaling. (1 or more = Wide) </summary>
        public float Ratio;

        /// <summary> The attached <seealso cref="RectTransform"/>. </summary>
        private RectTransform rectTransform;

        /// <summary>
        ///     Sets the ratio once again.
        /// </summary>
        private void UpdateRatio()
        {
            rectTransform.sizeDelta = Vector2.zero;

            Rect rect = rectTransform.rect;

            float width, height;
            
            if (Ratio > rect.width / rect.height)
            {
                width = rect.width;
                height = rect.width / Ratio;
            }
            else
            {
                width = rect.height * Ratio;
                height = rect.height;
            }

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        /// <summary>
        ///     Called by Unity to update the <see cref="UIRatioScalar"/> once per frame.
        /// </summary>
        private void Update()
        {
            UpdateRatio();
        }

        /// <summary>
        ///     Called by Unity to initialize the <see cref="UIRatioScalar"/>.
        /// </summary>
        private void Awake()
        {
            this.FetchComponent(ref rectTransform);
        }
    }
}

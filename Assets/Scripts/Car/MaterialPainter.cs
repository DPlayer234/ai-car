using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DPlay.AICar.Car
{
    /// <summary>
    ///     Paints all specified <see cref="Renderer"/>s' <see cref="Material"/>s with the specified color.
    /// </summary>
    public class MaterialPainter : MonoBehaviour
    {
        /// <summary> The <see cref="Renderer"/>s to change the <seealso cref="Material"/> of. </summary>
        public Renderer[] Renderers;

        /// <summary> Internal value to store the color for the <seealso cref="Material"/> or to be set in the editor. </summary>
        [SerializeField]
        private Color color;

        /// <summary>
        ///     The Color of the <seealso cref="Renderer"/> <see cref="Material"/>s.
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
                ApplyColor();
            }
        }

        /// <summary>
        ///     Applies the <see cref="Color"/>.
        /// </summary>
        private void ApplyColor()
        {
            foreach (Renderer renderer in Renderers)
            {
                Material newMaterial = new Material(renderer.material)
                {
                    color = Color
                };

                renderer.material = newMaterial;
            }
        }

        /// <summary>
        ///     Called by Unity once to initialize the <seealso cref="MaterialPainter"/>.
        ///     Applies the color immediately.
        /// </summary>
        private void Awake()
        {
            ApplyColor();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DPlay.AICar.Car
{
    public class MaterialPainter : MonoBehaviour
    {
        public Renderer[] Renderers;

        [SerializeField]
        private Color color;

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

        private void Awake()
        {
            ApplyColor();
        }

        private void OnValidate()
        {
            // ApplyColor();
        }
    }
}

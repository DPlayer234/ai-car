using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DPlay.AICar.Car
{
    public class CameraController : MonoBehaviour
    {
        public Transform ToFollow;
        public float ForwardMultiplicatorOffset;
        public Vector3 PositionOffset;
        public Vector3 RotationOffset;

        [Range(0.0f, 0.95f)]
        public float PositionApproachFactor;

        public void UpdatePosition(float deltaTime)
        {
            // Approach Target Position
            transform.position = HelperFunctions.ExpApproach(
                transform.position,  // Current Position
                ToFollow.position + ToFollow.forward * ForwardMultiplicatorOffset + PositionOffset,  // Target Position
                PositionApproachFactor,
                deltaTime);

            // Set Target Angle
            transform.eulerAngles = ToFollow.eulerAngles + RotationOffset;
        }

        private void Update()
        {
            UpdatePosition(Time.deltaTime);
        }

        private void Awake()
        {
            if (ToFollow == null)
            {
                Destroy(this);
                throw new NullReferenceException("ToFollow was not set.");
            }
        }

        private void OnValidate()
        {
            if (ToFollow == null) return;

            UpdatePosition(Mathf.Infinity);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DPlay.AICar.Car
{
    /// <summary>
    ///     Controls the camera (own transform) to follow something.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        /// <summary> The <see cref="Transform"/> to follow. </summary>
        public Transform ToFollow;

        /// <summary> By which factor <seealso cref="ToFollow.forward"/> is multiplied to be used as an offset. </summary>
        public float ForwardMultiplicatorOffset;

        /// <summary> Flat position offset. </summary>
        public Vector3 PositionOffset;

        /// <summary> Flat rotation offset. </summary>
        public Vector3 RotationOffset;

        /// <summary> How quickly the new position is approached. Range: [0.0..1.0] (Lower values are faster) </summary>
        [Range(0.0f, 0.95f)]
        public float PositionApproachFactor;

        /// <summary>
        ///     Updates the position.
        /// </summary>
        /// <param name="deltaTime">The time passed since the last update.</param>
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

        /// <summary>
        ///     Called by Unity to update the <seealso cref="CameraController"/> each frame.
        /// </summary>
        private void Update()
        {
            UpdatePosition(Time.deltaTime);
        }

        /// <summary>
        ///     Called by Unity once to initialize the <seealso cref="CameraController"/>.
        /// </summary>
        private void Awake()
        {
            if (ToFollow == null)
            {
                Destroy(this);
                throw new NullReferenceException("ToFollow was not set.");
            }
        }

        /// <summary>
        ///     Called by Unity when any values of the <seealso cref="CameraController"/> are changed in the editor.
        /// </summary>
        private void OnValidate()
        {
            if (ToFollow == null) return;

            UpdatePosition(Mathf.Infinity);
        }
    }
}

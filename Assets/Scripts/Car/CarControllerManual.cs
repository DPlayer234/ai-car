using UnityEngine;

namespace DPlay.AICar.Car
{
    /// <summary>
    ///     Allows the car to be controlled via the arrow keys/WASD/whatever is set as the Horizontal/Vertical axis.
    /// </summary>
    public class CarControllerManual : CarController
    {
        /// <summary>
        ///     Gets the linear speed input. Vertical Axis Input.
        /// </summary>
        /// <returns>The input value.</returns>
        public override float GetLinearSpeedInput()
        {
            return Input.GetAxis("Vertical");
        }

        /// <summary>
        ///     Gets the angular speed input. Horizontal Axis Input.
        /// </summary>
        /// <returns>The input value.</returns>
        public override float GetAngularSpeedInput()
        {
            return Input.GetAxis("Horizontal");
        }
    }
}

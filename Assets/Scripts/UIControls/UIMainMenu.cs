using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DPlay.AICar.UIControls
{
    /// <summary>
    ///     Contains function to be used in the Main Menu scene.
    /// </summary>
    public class UIMainMenu : MonoBehaviour
    {
        /// <summary>
        ///     Exits the application.
        /// </summary>
        public void ExitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

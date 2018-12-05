using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace DPlay.AICar
{
    /// <summary>
    ///     Provides static functions to load a scene.
    ///     May also be used as a <seealso cref="Component"/> to make buttons load scenes.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        /// <summary> The scene to load. </summary>
        public Scene ToLoad;

        /// <summary> The way to load the scene. </summary>
        public LoadSceneMode LoadMode;

        /// <summary> An event to trigger once the scene is loaded. </summary>
        public UnityEvent OnSceneLoaded;

        /// <summary>
        ///     Contains values indicating the existing scenes.
        /// </summary>
        public enum Scene
        {
            /// <summary> Represents the Main Menu scene. </summary>
            MainMenu,

            /// <summary> Represents the Test Drive scene. </summary>
            TestDrive,

            /// <summary> Represents the Evolution scene. </summary>
            Evolution
        }

        /// <summary>
        ///     Delegate type for functions to be called when a scene is loaded.
        /// </summary>
        public delegate void SceneLoadedCallback();

        /// <summary>
        ///     Loads a scene asynchronously.
        /// </summary>
        /// <param name="scene">The scene to load.</param>
        /// <param name="loadMode">The way to load the scene.</param>
        public static void LoadScene(Scene scene, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            LoadSceneInternal(scene, loadMode);
        }

        /// <summary>
        ///     Loads a scene asynchronously.
        /// </summary>
        /// <param name="scene">The scene to load.</param>
        /// <param name="onSceneLoaded">The callback to call when the scene is loaded.</param>
        /// <param name="loadMode">The way to load the scene.</param>
        public static void LoadScene(Scene scene, SceneLoadedCallback onSceneLoaded, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            AsyncOperation operation = LoadSceneInternal(scene, loadMode);

            operation.completed += (o) => onSceneLoaded();
        }

        /// <summary>
        ///     Loads the scene asynchronously based on the instance fields.
        /// </summary>
        public void LoadScene()
        {
            LoadScene(ToLoad, OnSceneLoaded.Invoke, LoadMode);
        }

        /// <summary>
        ///     Common call for loading scenes.
        /// </summary>
        /// <param name="scene">The scene to load.</param>
        /// <param name="loadMode">The way to load the scene.</param>
        /// <returns>The <seealso cref="AsyncOperation"/> started by loading the scene.</returns>
        private static AsyncOperation LoadSceneInternal(Scene scene, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            return SceneManager.LoadSceneAsync(Enum.GetName(typeof(Scene), scene), loadMode);
        }
    }
}

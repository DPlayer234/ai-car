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
    public class SceneLoader : MonoBehaviour
    {
        public Scene ToLoad;

        public LoadSceneMode LoadMode;

        public UnityEvent OnSceneLoaded;

        public enum Scene
        {
            MainMenu,
            TestDrive,
            Evolution
        }

        public delegate void SceneLoadedCallback();

        public static void LoadScene(Scene scene, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            LoadSceneInternal(scene, loadMode);
        }

        public static void LoadScene(Scene scene, SceneLoadedCallback onSceneLoaded, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            AsyncOperation operation = LoadSceneInternal(scene, loadMode);

            operation.completed += (o) => onSceneLoaded();
        }

        public void LoadScene()
        {
            LoadScene(ToLoad, OnSceneLoaded.Invoke, LoadMode);
        }

        private static AsyncOperation LoadSceneInternal(Scene scene, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            return SceneManager.LoadSceneAsync(Enum.GetName(typeof(Scene), scene), loadMode);
        }
    }
}

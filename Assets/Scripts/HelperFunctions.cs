using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DPlay.AICar
{
    public static class HelperFunctions
    {
        /// <summary>
        ///     Gets a <seealso cref="Component"/> of the specified type from the <seealso cref="MonoBehaviour"/> and assigns it to the given <paramref name="variable"/>.
        /// </summary>
        /// <exception cref="NullReferenceException">No such <seealso cref="Component"/> is attached.</exception>
        /// <typeparam name="T">The type of the <seealso cref="Component"/>.</typeparam>
        /// <param name="self">The <seealso cref="MonoBehaviour"/> to get the <seealso cref="Component"/> from.</param>
        /// <param name="variable">The variable to assign the <seealso cref="Component"/> to.</param>
        public static void FetchComponent<T>(this MonoBehaviour self, ref T variable)
        {
            T component = self.GetComponent<T>();

            if (component == null)
            {
                throw new NullReferenceException("No such Component is attached.");
            }

            variable = component;
        }

        public static float ExpApproach(float current, float target, float accelerationFactor, float deltaTime)
        {
            return target - (target - current) * Mathf.Pow(accelerationFactor, deltaTime);
        }

        public static Vector3 ExpApproach(Vector3 current, Vector3 target, float accelerationFactor, float deltaTime)
        {
            return target - (target - current) * Mathf.Pow(accelerationFactor, deltaTime);
        }

        public static Vector3 RotateAroundY(Vector3 v0, float angle)
        {
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

            return new Vector3(
                cos * v0.x + sin * v0.z,
                v0.y,
                cos * v0.z - sin * v0.x);
        }

        public static string GetPathTo(string fileName)
        {
            return Application.persistentDataPath + "/" + fileName;
        }

        public static T[] Slice<T>(this T[] self, int from, int to)
        {
            T[] slice = new T[to - from];

            int dest = 0;
            for (int src = from; src < to; src++)
            {
                slice[dest] = self[src];
                dest++;
            }

            return slice;
        }

        public static double[] ToDoubleArray(this float[] source)
        {
            double[] result = new double[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                result[i] = (double)source[i];
            }

            return result;
        }

        public static T[] CopyArray<T>(this T[] src)
        {
            T[] dest = new T[src.Length];

            for (int i = 0; i < src.Length; i++)
            {
                dest[i] = src[i];
            }

            return dest;
        }
        
        public static TDest[] CopyAs<TSrc, TDest>(this TSrc[] src) where TSrc : TDest
        {
            TDest[] dest = new TDest[src.Length];

            for (int i = 0; i < src.Length; i++)
            {
                dest[i] = src[i];
            }

            return dest;
        }

        public static void DebugLogArray<T>(T[] array)
        {
            StringBuilder buffer = new StringBuilder();

            foreach (var item in array)
            {
                buffer.Append(item).Append(", ");
            }

            Debug.Log(buffer);
        }
    }
}

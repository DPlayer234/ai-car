using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DPlay.AICar
{
    /// <summary>
    ///     Static class containing various helper functions.
    /// </summary>
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

        /// <summary>
        ///     Calculates an exponential approach to a specified value.
        /// </summary>
        /// <param name="current">The current value</param>
        /// <param name="target">The target value or "limit"</param>
        /// <param name="accelerationFactor">The acceleration factor</param>
        /// <param name="deltaTime">The time that has passed since the last related call.</param>
        /// <returns>The new value.</returns>
        public static float ExpApproach(float current, float target, float accelerationFactor, float deltaTime)
        {
            return target - (target - current) * Mathf.Pow(accelerationFactor, deltaTime);
        }

        /// <summary>
        ///     Calculates an exponential approach to a specified value.
        /// </summary>
        /// <param name="current">The current value</param>
        /// <param name="target">The target value or "limit"</param>
        /// <param name="accelerationFactor">The acceleration factor</param>
        /// <param name="deltaTime">The time that has passed since the last related call.</param>
        /// <returns>The new value.</returns>
        public static Vector3 ExpApproach(Vector3 current, Vector3 target, float accelerationFactor, float deltaTime)
        {
            return target - (target - current) * Mathf.Pow(accelerationFactor, deltaTime);
        }

        /// <summary>
        ///     Rotates a vector around the Y axis by the specified angle in degrees.
        /// </summary>
        /// <param name="v0">The current vector</param>
        /// <param name="angle">The angle in degrees</param>
        /// <returns>A new vector equal to v0 rotated by angle degrees</returns>
        public static Vector3 RotateAroundY(Vector3 v0, float angle)
        {
            float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
            float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

            return new Vector3(
                cos * v0.x + sin * v0.z,
                v0.y,
                cos * v0.z - sin * v0.x);
        }
        
        /// <summary>
        ///     Gets the path to a file in the persistent data path.
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <returns>The absolute path to the file</returns>
        public static string GetPersistentPathTo(string fileName)
        {
            return Application.persistentDataPath + "/" + fileName;
        }

        /// <summary>
        ///     Returns a slice from an array.
        /// </summary>
        /// <typeparam name="T">The type of the values in the array.</typeparam>
        /// <param name="self">The array to slice from.</param>
        /// <param name="from">The inclusive beginning index of the slice.</param>
        /// <param name="to">The exclusive ending index of the slice.</param>
        /// <returns>A new array with the values in the specified area of the array.</returns>
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

        /// <summary>
        ///     Converts an array of <seealso cref="float"/>s to an array of <seealso cref="double"/>s.
        /// </summary>
        /// <param name="source">The source array.</param>
        /// <returns>A new array with identical values but a different data type.</returns>
        public static double[] ToDoubleArray(this float[] source)
        {
            double[] result = new double[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                result[i] = (double)source[i];
            }

            return result;
        }

        /// <summary>
        ///     Creates a new array with all values from <paramref name="src"/> copied into it.
        /// </summary>
        /// <typeparam name="T">The type of the values in the array.</typeparam>
        /// <param name="src">The source array.</param>
        /// <returns>A copy of the source array.</returns>
        public static T[] CopyArray<T>(this T[] src)
        {
            T[] dest = new T[src.Length];

            for (int i = 0; i < src.Length; i++)
            {
                dest[i] = src[i];
            }

            return dest;
        }
        
        /// <summary>
        ///     Copies an array as one with a different contained data type.
        /// </summary>
        /// <typeparam name="TSrc">The source data type.</typeparam>
        /// <typeparam name="TDest">The destination data type.</typeparam>
        /// <param name="src">The source array.</param>
        /// <returns>A new array with the same values diguised as a different data type.</returns>
        public static TDest[] CopyAs<TSrc, TDest>(this TSrc[] src) where TSrc : TDest
        {
            TDest[] dest = new TDest[src.Length];

            for (int i = 0; i < src.Length; i++)
            {
                dest[i] = src[i];
            }

            return dest;
        }

        /// <summary>
        ///     Uses <see cref="Debug.Log(string)"/> to log the contents of an array.
        /// </summary>
        /// <typeparam name="T">The type of data in the array.</typeparam>
        /// <param name="array">The array to log.</param>
        public static void DebugLogArray<T>(T[] array)
        {
            StringBuilder buffer = new StringBuilder();

            foreach (var item in array)
            {
                buffer.Append(item).Append(", ");
            }

            Debug.Log(buffer);
        }

        /// <summary>
        ///     Creates a random color.
        /// </summary>
        /// <returns>A random color.</returns>
        public static Color GetRandomColor()
        {
            byte[] colorBytes = new byte[3];
            Globals.Random.NextBytes(colorBytes);

            return new Color(
                colorBytes[0] / 255.0f,
                colorBytes[1] / 255.0f,
                colorBytes[2] / 255.0f);
        }

        /// <summary>
        ///     Gets a random float in the supplied range.
        /// </summary>
        /// <param name="maximum">The minimum possible value.</param>
        /// <param name="minimum">The maximum possible value.</param>
        /// <returns>A random float.</returns>
        public static float GetRandomFloat(float minimum, float maximum)
        {
            float difference = maximum - minimum;
            return (float)(Globals.Random.NextDouble() * difference - difference * 0.5);
        }
    }
}

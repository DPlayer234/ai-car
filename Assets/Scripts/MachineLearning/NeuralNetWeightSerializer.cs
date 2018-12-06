using System;

namespace DPlay.AICar.MachineLearning
{
    /// <summary>
    ///     Serializes weights to arrays of bytes or a string format.
    /// </summary>
    public static class NeuralNetWeightSerializer
    {
        /// <summary>
        ///     Copies an array of doubles to an array of bytes.
        /// </summary>
        /// <param name="weights">The weights to convert.</param>
        /// <returns>An array of bytes, containing the raw data of the doubles.</returns>
        public static byte[] ToByteArray(double[] weights)
        {
            return CopyData<double, byte>(weights, sizeof(double), sizeof(byte));
        }

        /// <summary>
        ///     Copies an array of bytes to an array of doubles.
        /// </summary>
        /// <param name="weightBytes">The raw bytes to convert back.</param>
        /// <returns>An array of doubles, containing the raw data of the bytes.</returns>
        public static double[] ToWeights(byte[] weightBytes)
        {
            return CopyData<byte, double>(weightBytes, sizeof(byte), sizeof(double));
        }

        /// <summary>
        ///     Converts an array of doubles to a weight code.
        /// </summary>
        /// <param name="weights">The weights to convert.</param>
        /// <returns>A weight code in hexidecimal format.</returns>
        public static string ToWeightCode(double[] weights)
        {
            byte[] bytes = ToByteArray(weights);
            char[] characters = new char[bytes.Length * 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                byte byteValue = bytes[i];
                string hexSub = byteValue.ToString("x2");

                characters[i * 2] = hexSub[0];
                characters[i * 2 + 1] = hexSub[1];
            }

            return new string(characters);
        }

        /// <summary>
        ///     Converts a weight code to a list of doubles.
        /// </summary>
        /// <param name="weightCode">The weight code. It should be a hexidecimal number with an even number of digits.</param>
        /// <returns>An array of doubles.</returns>
        /// <exception cref="FormatException">The specified string is not a series of hexadecimal digits.</exception>
        /// <exception cref="FormatException">The code length is not evenly divisible by 2.</exception>
        public static double[] ToWeights(string weightCode)
        {
            if (weightCode.Length % 2 != 0)
            {
                throw new FormatException("weightCode has to be of a length divisible by 2.");
            }

            byte[] bytes = new byte[weightCode.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                string sub = weightCode.Substring(i * 2, 2);
                bytes[i] = Convert.ToByte(sub, 16);
            }

            return ToWeights(bytes);
        }

        /// <summary>
        ///     Copies raw bytes from one array to another.
        /// </summary>
        /// <typeparam name="TSrc">The source data type.</typeparam>
        /// <typeparam name="TDest">The destination data type.</typeparam>
        /// <param name="data">The original data.</param>
        /// <param name="sizeOfTSrc">The sizeof(<typeparamref name="TSrc"/>).</param>
        /// <param name="sizeOfTDest">The sizeof(<typeparamref name="TDest"/>).</param>
        /// <returns>The data copied into a different array.</returns>
        private static TDest[] CopyData<TSrc, TDest>(TSrc[] data, int sizeOfTSrc, int sizeOfTDest)
            where TSrc : struct
            where TDest : struct
        {
            int byteCount = data.Length * sizeOfTSrc;
            TDest[] bytes = new TDest[byteCount / sizeOfTDest];

            Buffer.BlockCopy(data, 0, bytes, 0, byteCount);

            return bytes;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSXMMCLibrary.Utilities
{
    public static class ParseHelper
    {
        private ParseHelper()
        {
            throw new NotImplementedException("This class is a static utility class and may not be instantiated.");
        }

        /// <summary>
        /// Returns a subrange from the given array.
        /// Source: http://stackoverflow.com/a/943650
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>
        /// Decode a Shift-JIS encoded string from raw byte data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string DecodeShiftJISString(this byte[] data, int index, int length)
        {
            return Constants.ShiftJisEncoding.GetString(data, index, length);
        }

        /// <summary>
        /// Represents an array's data as a readable string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ArrayToString<T>(this T[] data)
        {
            return string.Join(", ", data.Select(x => x.ToString()).ToArray());
        }

        /// <summary>
        /// Convert a byte value to an 8-bit boolean array.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool[] To8BitArray(this byte value)
        {
            bool[] result = new bool[32];
            BitArray bitArray = new BitArray(new int[] { value });
            bitArray.CopyTo(result, 0);

            return result.SubArray(0, 8);
        }

        /// <summary>
        /// Convert 8-bits (in boolean array form) into a byte value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ToByte(this bool[] value)
        {
            byte result = 0;

            for (int i = 0; i < value.Length; ++i)
            {
                if (i == 0 && !value[i]) { continue; }

                if (value[i])
                {
                    result += (byte)Math.Pow(2, i);
                }
            }

            return result;
        }

        /// <summary>
        /// Get an integer value from a subarray of a byte's bits
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ushort SubByte(this byte value, int index, int length)
        {
            bool[] bits = value.To8BitArray();
            ushort result = 0;

            for (int i = index, j = 0; i < index + length; ++i, ++j)
            {
                if (j == 0 && !bits[i]) { continue; }

                if (bits[i])
                {
                    result += (ushort)Math.Pow(2, j);
                }
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSXMMCLibrary
{
    public static class ParseHelper
    {
        /// <summary>
        /// The PSX memory card's default encoding for strings it displays.
        /// </summary>
        private static readonly Encoding _shiftJisEncoding = Encoding.GetEncoding(932);

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
            return _shiftJisEncoding.GetString(data, index, length);
        }
    }
}

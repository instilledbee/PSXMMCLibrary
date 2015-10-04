﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSXMMCLibrary
{
    /// <summary>
    /// Values related to PSX specifications
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The PSX memory card's default encoding for strings it displays.
        /// </summary>
        public static readonly Encoding ShiftJisEncoding = Encoding.GetEncoding(932);

        /// <summary>
        /// The length of a single memory card block, in bytes.
        /// </summary>
        public static readonly uint BlockLength = 8192;

        /// <summary>
        /// The length of a single block frame, in bytes.
        /// </summary>
        public static readonly int FrameLength = 128;

        /// <summary>
        /// The number of blocks in a memory card file.
        /// </summary>
        public static readonly int BlockCount = 15;
    }
}

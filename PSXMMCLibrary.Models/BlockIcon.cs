using System;
using System.Collections.Generic;
using System.Drawing;

namespace PSXMMCLibrary.Models
{
    /// <summary>
    /// Represents a block's icon frames and colors
    /// </summary>
    public class BlockIcon
    {
        /// <summary>
        /// Color lookup table for icon pixels
        /// </summary>
        public Color[] Colors { get; set; }

        /// <summary>
        /// Pixel data for each individual icon frame
        /// </summary>
        public List<ushort[]> Frames { get; set; }
    }
}

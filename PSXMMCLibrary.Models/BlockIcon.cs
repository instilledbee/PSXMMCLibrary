using System.Collections.Generic;
using System.Drawing;

namespace PSXMMCLibrary.Models
{
    /// <summary>
    /// Represents a block's icon frames and colors
    /// </summary>
    public class BlockIcon
    {
        public BlockIcon()
        {
            this.Frames = new List<ushort[]>();
            this.Colors = new List<Color>();
        }

        /// <summary>
        /// Color lookup table for icon pixels
        /// </summary>
        public List<Color> Colors { get; private set; }

        /// <summary>
        /// Pixel data for each individual icon frame
        /// </summary>
        public List<ushort[]> Frames { get; private set; }
    }
}

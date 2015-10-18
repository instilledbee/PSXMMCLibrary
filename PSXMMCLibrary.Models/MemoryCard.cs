using System.Collections.Generic;

namespace PSXMMCLibrary.Models
{
    /// <summary>
    /// Represents a group of save game data
    /// </summary>
    public class MemoryCard
    {
        /// <summary>
        /// The saved games contained in the memory card
        /// </summary>
        public List<Block> Blocks { get; private set; }

        /// <summary>
        /// Metadata about the saved games in the memory card
        /// </summary>
        public List<DirectoryFrame> DirectoryFrames { get; private set; }

        /// <summary>
        /// The filename of the memory card file as saved in disk
        /// </summary>
        public string FileName { get; set; }
    }
}

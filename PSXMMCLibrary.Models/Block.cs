namespace PSXMMCLibrary.Models
{
    /// <summary>
    /// Model that represents a block of memory card data
    /// </summary>
    public class Block
    {
        /// <summary>
        /// Number of animation frames the block's icon contains
        /// </summary>
        public int IconFrames { get; set; }

        /// <summary>
        /// Number of blocks the game save uses
        /// </summary>
        public int BlocksUsed { get; set; }

        /// <summary>
        /// The title of the save as displayed
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The block's icon colors and frames
        /// </summary>
        public BlockIcon Icon { get; set; }

        /// <summary>
        /// The save data for the game
        /// </summary>
        public byte[] SaveData { get; set; }

        /// <summary>
        /// If this is a link block, its raw data only contains game save data.
        /// </summary>
        public bool IsLinkBlock { get; set; }
    }
}

using PSXMMCLibrary.Models.Enums;

namespace PSXMMCLibrary.Models
{
    /// <summary>
    /// Contains metadata about a save game block
    /// </summary>
    public class DirectoryFrame
    {
        /// <summary>
        /// Current usage status of the referenced block.
        /// </summary>
        public AvailableStatus AvailableStatus { get; set; }

        /// <summary>
        /// Number of blocks the save game will be using
        /// </summary>
        public int BlocksUsed { get; set; }

        /// <summary>
        /// The position of the block in its link line.
        /// </summary>
        public int LinkOrder { get; set; }

        /// <summary>
        /// The saved game's country code
        /// </summary>
        public CountryCode Country { get; set; }

        /// <summary>
        /// The saved game's product code
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// The saved game's identifier
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Alias property for concatenating the country code, product code and identifier
        /// </summary>
        public string Filename 
        {
            get 
            {
                return Country.ToString() + ProductCode + Identifier;
            }
        }

        /// <summary>
        /// XOR operation of all bytes of the frame
        /// </summary>
        public byte CheckSum { get; set; }
    }
}

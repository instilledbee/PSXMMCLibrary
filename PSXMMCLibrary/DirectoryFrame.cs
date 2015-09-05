using PSXMMCLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSXMMCLibrary
{
    /// <summary>
    /// Contains metadata about a save game block
    /// </summary>
    public class DirectoryFrame
    {
        private static readonly uint _FRAME_DATA_LENGTH = 128;

        /// <summary>
        /// Current usage status of the referenced block.
        /// </summary>
        public AvailableStatus AvailableFlag { get; private set; }

        /// <summary>
        /// Number of blocks the save game will be using
        /// </summary>
        public int BlocksUsed { get; private set; }

        /// <summary>
        /// The position of the block in its link line.
        /// </summary>
        public int LinkOrder { get; private set; }

        /// <summary>
        /// The saved game's country code
        /// </summary>
        public CountryCode Country { get; private set; }

        /// <summary>
        /// The saved game's product code
        /// </summary>
        public string ProductCode { get; private set; }

        /// <summary>
        /// The saved game's identifier
        /// </summary>
        public string Identifier { get; private set; }

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
        public byte CheckSum { get; private set; }

        /// <summary>
        /// Constructor is hidden from outside the assembly.
        /// </summary>
        internal DirectoryFrame() { }

        /// <summary>
        /// Create a new DirectoryFrame structure from raw memory card data
        /// </summary>
        /// <param name="data">A 128-length array containing the raw data</param>
        /// <returns></returns>
        public static DirectoryFrame Parse(byte[] data)
        {
            DirectoryFrame frame = new DirectoryFrame();

            try
            {
                // Sanity checks
                Contract.Requires<ArgumentNullException>(data != null);
                Contract.Requires<ArgumentException>(data.Length == _FRAME_DATA_LENGTH);

                frame.AvailableFlag = ParseAvailableStatus(data[0]);
                frame.BlocksUsed = ParseBlocksUsed(data.SubArray(4, 4));
                frame.LinkOrder = ParseLinkOrder(data.SubArray(8, 2));

                // Only first link blocks have this data
                if (frame.AvailableFlag == AvailableStatus.FirstLink)
                {
                    frame.Country = ParseCountryCode(data.SubArray(10, 2));
                    frame.ProductCode = ParseProductCode(data.SubArray(12, 10));
                    frame.Identifier = ParseIdentifier(data.SubArray(22, 8));
                }

                // TODO: Validate checksum by figuring out how the XOR on the bytes work.
                frame.CheckSum = data[127];
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Error parsing directory frame data.");
                Console.WriteLine(ex.ToString());
                frame = null;
            }

            return frame;
        }

        private static AvailableStatus ParseAvailableStatus(byte availableByte)
        {
            /*
             * (+0x00)
             * Available blocks (Also used on PocketStation to retrieve use and empty block.):
             * A0 (Open block)
             * 51 (In use, there will be a link in the next block)
             * 52 (In use, this is in a link and will link to another)
             * 53 (In use, this is the last in the link) 
             * FF (Unusable)
             * 
             * Linked blocks on a memory card seem to be a rudimentary singly-linked-list implementation.
             * 
             * Length: 1 byte
             */
            switch (availableByte)
            {
                case 160:
                    return AvailableStatus.OpenBlock;

                case 81:
                    return AvailableStatus.FirstLink;

                case 82:
                    return AvailableStatus.MiddleLink;

                case 83:
                    return AvailableStatus.LastLink;

                case 255:
                    return AvailableStatus.Unusable;

                default:
                    throw new FormatException("Invalid available byte value");
            }
        }

        private static int ParseBlocksUsed(byte[] useBytes)
        {
            /*
             * (+0x04 - +0x07)
             * Used Block Bytes
             * (00 20 00 00) - one block will be used
             * (00 40 00 00) - two blocks will be used
             * (00 E0 01 00) - 15 blocks will be used (max)
             * 
             * The count seems to start with the second byte, although not sure what the other bytes are for
             * May need to do more testing with saves that are more than 7 blocks.
             * Not sure what the first and fourth bytes are for.
             * 
             * Length: 4 bytes
             */
            Contract.Requires(useBytes.Length == 4);

            int baseCount = (Convert.ToInt32(useBytes[1].ToString("X"), 16)) / 7;

            // TODO: Figure out how other bytes represent saves that are more than 7 blocks long.

            return baseCount;
        }

        private static int ParseLinkOrder(byte[] linkOrderBytes)
        {
            /*
             * (+0x08 - +0x09)
             * Link order
             * If the block/frame isn't in a link or if it's the last link in the line, it's 0xFFFF
             * 
             * Otherwise, shows the position of the block in the directory.
             * 
             * Length: 2 bytes
             */
            Contract.Requires(linkOrderBytes.Length == 2);

            if (linkOrderBytes[0] == 255 && linkOrderBytes[1] == 255)
            {
                return -1;
            }
            else
            {
                return (Convert.ToInt32(linkOrderBytes[1].ToString("X"), 16));
            }
        }

        private static CountryCode ParseCountryCode(byte[] countryCodeBytes)
        {
            /*
             * (+0x0A - +0x0B)
             * Country code
             * (Japan = BI, America = BA, Europe = BE) 
             *
             * Length: 2 bytes
             */
            Contract.Requires(countryCodeBytes.Length == 2);

            var countryCodeString = countryCodeBytes.DecodeShiftJISString(0, 2);

            return (CountryCode)Enum.Parse(typeof(CountryCode), countryCodeString);
        }

        private static string ParseProductCode(byte[] productCodeBytes)
        {
            /*
             * (+0x0C - +0x15)
             * Product Code/Disc ID (AAAA-00000)
             * Japan SLPS, SCPS (from SCEI)
             * America SLUS, SCUS (from SCEA)
             * Europe SLES, SCES (from SCEE)
             * 
             * Length: 10 bytes
             */
            Contract.Requires(productCodeBytes.Length == 10);

            return productCodeBytes.DecodeShiftJISString(0, 10);
        }

        private static string ParseIdentifier(byte[] identifierBytes)
        {
            /*
             * (+0x16 - +0x1D)
             * Identifier
             * Created unique to the session of the save game.
             * 
             * Length: 8 bytes
             */
            Contract.Requires(identifierBytes.Length == 8);

            return identifierBytes.DecodeShiftJISString(0, 8);
        }
    }
}

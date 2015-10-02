using PSXMMCLibrary.Models;
using PSXMMCLibrary.Models.Enums;
using PSXMMCLibrary.Utilities;
using System;

namespace PSXMMCLibrary
{
    public class DirectoryFrameParser
    {
        private static readonly uint _FRAME_DATA_LENGTH = 128;

        /// <summary>
        /// Create a new DirectoryFrame structure from raw memory card data
        /// </summary>
        /// <param name="data">A 128-length array containing the raw data</param>
        /// <returns></returns>
        public static DirectoryFrame Parse(byte[] data)
        {
            DirectoryFrame frame = null;

            try
            {
                frame = new DirectoryFrame();

                // Sanity checks
                Contract.Requires<ArgumentNullException>(data != null);
                Contract.Requires<ArgumentException>(data.Length == _FRAME_DATA_LENGTH);

                frame.AvailableStatus = ParseAvailableStatus(data[0]);
                frame.BlocksUsed = ParseBlocksUsed(data.SubArray(4, 4));
                frame.LinkOrder = ParseLinkOrder(data.SubArray(8, 2));

                // Only first link blocks have this data
                if (frame.AvailableStatus == AvailableStatus.FirstLink)
                {
                    frame.Country = ParseCountryCode(data.SubArray(10, 2));
                    frame.ProductCode = ParseProductCode(data.SubArray(12, 10));
                    frame.Identifier = ParseIdentifier(data.SubArray(22, 8));
                }

                // TODO: Validate checksum by figuring out how the XOR on the bytes work.
                // Each byte is XORed one by one and the result is stored. Complies with the checksum protocol.
                frame.CheckSum = data[127];
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid data passed to parser.");
                Console.WriteLine(ex.Message);

                throw ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing directory frame data.");
                Console.WriteLine(ex.Message);

                throw ex;
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
            Contract.Requires<ArgumentException>(useBytes.Length == 4);

            int baseCount = (Convert.ToInt32(useBytes[1].ToString("X"), 16)) / 32;

            // TODO: Figure out how other bytes represent saves that are more than 7 blocks long.
            if (Convert.ToInt32(useBytes[2].ToString("X"), 16) == 1)
            {
                baseCount += 8;
            }

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
            Contract.Requires<ArgumentException>(linkOrderBytes.Length == 2);

            int linkOrder = 0;

            if (linkOrderBytes[0] == 255 && linkOrderBytes[1] == 255)
            {
                linkOrder = -1;
            }
            else
            {
                linkOrder = Convert.ToInt32(linkOrderBytes[1].ToString("X"), 16);

                if (linkOrder > 15 || linkOrder < 0)
                {
                    throw new FormatException("Link order is not a valid block index.");
                }
            }

            return linkOrder;
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
            Contract.Requires<ArgumentException>(countryCodeBytes.Length == 2);

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
            Contract.Requires<ArgumentException>(productCodeBytes.Length == 10);

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
            Contract.Requires<ArgumentException>(identifierBytes.Length == 8);

            return identifierBytes.DecodeShiftJISString(0, 8);
        }
    }
}

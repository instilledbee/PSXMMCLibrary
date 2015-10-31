using PSXMMCLibrary.Models;
using PSXMMCLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PSXMMCLibrary
{
    public static class BlockParser
    {
        /// <summary>
        /// Create a new Block model from raw memory card data
        /// </summary>
        /// <param name="data">An 8192-length byte array</param>
        /// <returns></returns>
        public static Block Parse(byte[] data) 
        {
            Block parsedBlock = null;

            try 
	        {
                parsedBlock = new Block();

                // Sanity checks
                Contract.Requires<ArgumentNullException>(data != null);
                Contract.Requires<ArgumentException>(data.Length == Constants.BlockLength);

                int frameBytesOffset = 0;

                parsedBlock.IsLinkBlock = !HasMagicValues(data.SubArray(0, 2));
                parsedBlock.BlocksUsed = ParseBlocksUsed(data[3]);
                parsedBlock.Title = ParseSaveTitle(data.SubArray(4, 64));

                if (!parsedBlock.IsLinkBlock)
                {
                    parsedBlock.IconFrames = ParseIconFrameCount(data[2]);
                }

                if (parsedBlock.IconFrames > 0 && !parsedBlock.IsLinkBlock)
                {
                    parsedBlock.Icon = new BlockIcon();
                    parsedBlock.Icon.Colors.AddRange(ParseColorData(data.SubArray(96, 32)));
                    parsedBlock.Icon.Frames.AddRange(ParseIconFrameData(data.SubArray(128, 128 * parsedBlock.IconFrames)));

                    frameBytesOffset = 128 * parsedBlock.IconFrames;
                }

                parsedBlock.SaveData = data.SubArray(128 + frameBytesOffset, (int)Constants.BlockLength - 128 - frameBytesOffset);
	        }
	        catch (ArgumentException ex)
	        {
                Console.WriteLine("Invalid raw block data passed to parser.");
                Console.WriteLine(ex.Message);
		        throw ex;
	        }
            catch (Exception ex)
            {
                Console.WriteLine("Error in parsing raw block data.");
                Console.WriteLine(ex.Message);
                throw ex;
            }

            return parsedBlock;
        }

        private static bool HasMagicValues(byte[] magicValues)
        {
            Contract.Requires<ArgumentException>(magicValues.Length == 2);

            return ((char)magicValues[0] == 'S') && ((char)magicValues[1] == 'C');
        }

        private static int ParseIconFrameCount(byte iconFrameByte)
        {
            int iconFrames = 0;

            switch (iconFrameByte)
            {
                case 0:
                    iconFrames = 0;
                    break;

                case 11:
                case 16:
                    iconFrames = 1;
                    break;

                case 12:
                case 17:
                    iconFrames = 2;
                    break;

                case 13:
                case 18:
                    iconFrames = 3;
                    break;

                    // TODO: Determine if valid
                case 19:
                    iconFrames = 4;
                    break;

                default:
                    throw new FormatException("Invalid icon frame byte value.");
            }

            return iconFrames;
        }

        private static int ParseBlocksUsed(byte blocksUsedByte)
        {
            return (int)(blocksUsedByte);
        }

        private static string ParseSaveTitle(byte[] saveTitleBytes)
        {
            Contract.Requires<ArgumentException>(saveTitleBytes.Length == 64);

            return saveTitleBytes.DecodeShiftJISString(0, 64);
        }

        private static Color[] ParseColorData(byte[] colorBytes)
        {
            Contract.Requires<ArgumentException>(colorBytes.Length == 32);

            /*
             * Color data is represented as:
             *  M     B     G    R
             * [15][14-10][9-5][4-0]
             *  1   00000 00000 00000
             * 
             * 4 bits per color channel
             * Last bit is used for pixel masking
             */

            Color[] colors = new Color[16];

            for (int i = 0, j = 0; i < 32; i += 2, ++j)
            {
                byte[] color = colorBytes.SubArray(i, 2);

                bool isOpaque = ((color[0] & 128) >> 7) == 1;

                int blue = color[0].SubByte(2, 5);

                // Merge bits from the two color value bytes to form value for green channel
                int green = color[0].To8BitArray()
                                    .SubArray(5, 3)
                                    .Concat(color[1].To8BitArray().SubArray(0, 2))
                                    .ToArray()
                                    .ToByte();

                int red = color[1].SubByte(0, 5);

                // Extrapolate the 5-bit color channels into 32-bit ARGB channels
                colors[j] = Color.FromArgb(isOpaque ? 255 : 0, red * 8, green * 8, blue * 8);
            }

            return colors;
        }

        private static List<ushort[]> ParseIconFrameData(byte[] frameData)
        {
            Contract.Requires<ArgumentException>(frameData.Length % 128 == 0);

            List<ushort[]> frames = new List<ushort[]>(frameData.Length / 128);

            for (int i = 0; i < frames.Capacity; ++i)
            {
                frames.Add(new ushort[256]);
                byte[] frameBytes = frameData.SubArray(i * 128, 128);

                for (int j = 0; j < frames[i].Length; j += 2)
                {
                    byte pixel = frameBytes[j / 2];
                    frames[i][j + 1] = pixel.SubByte(0, 4);
                    frames[i][j] = pixel.SubByte(4, 4);
                }
            }

            return frames;
        }
    }
}

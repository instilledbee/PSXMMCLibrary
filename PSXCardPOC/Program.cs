using System;
using System.IO;
using System.Text;
using PSXMMCLibrary;

namespace PSXCardPOC
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryCard mc = new MemoryCard("ctr.mcr");
            Encoding shiftJisEncoding = Encoding.GetEncoding(932);

            byte[] mBlock = mc.GetBlock(0);

            Console.WriteLine("Header Block: ");
            // Values should be "M" "C"
            Console.WriteLine("Magic values: {0}{1}", (char)(mBlock[0]), (char)(mBlock[1]));
            // XOR or magic values (should be "OE")
            Console.WriteLine("Checksum magic values: {0}", mBlock[127].ToString("X2"));

            Console.ReadKey();

            for (int i = 0; i <= 14; ++i)
            {
                var directoryFrame = mc.GetDirectoryFrame(i);
                Console.WriteLine("(#{0}) Directory Frame: ", i + 1);

                Console.WriteLine("Availability: {0}", directoryFrame.AvailableStatus);
                Console.WriteLine("Used Blocks Byte: {0}", directoryFrame.BlocksUsed);
                Console.WriteLine("Link order: {0}", directoryFrame.LinkOrder);
                Console.WriteLine("Country Code: {0}", directoryFrame.Country);
                Console.WriteLine("Product Code: {0}", directoryFrame.ProductCode);
                Console.WriteLine("Identifier: {0}", directoryFrame.Identifier);
                Console.WriteLine("Checksum: {0}", directoryFrame.CheckSum);
                Console.ReadKey();
            }

            //for (int i = 0; i <= 15; ++i)
            //{
            //    Console.WriteLine("(#{0}) Directory Frame: ", i);

            //    Console.WriteLine("Availability: {0}", mBlock[offset++].ToString("X2"));

            //    Console.Write("Block Reserved Flags:");
            //    Console.Write(" " + mBlock[offset++].ToString("X2"));
            //    Console.Write(" " + mBlock[offset++].ToString("X2"));
            //    Console.Write(" " + mBlock[offset++].ToString("X2"));
            //    Console.WriteLine("");

            //    Console.Write("Used Blocks Byte:");
            //    Console.Write(" " + mBlock[offset++].ToString("X2"));
            //    Console.Write(" " + mBlock[offset++].ToString("X2"));
            //    Console.Write(" " + mBlock[offset++].ToString("X2"));
            //    Console.Write(" " + mBlock[offset++].ToString("X2"));
            //    Console.WriteLine("");

            //    Console.Write("Link order:");
            //    Console.Write(" " + mBlock[offset++].ToString("X2"));
            //    Console.Write(" " + mBlock[offset++].ToString("X2"));
            //    Console.WriteLine("");

            //    Console.WriteLine("Country Code: {0}", mBlock.DecodeShiftJISString(offset, 2));
            //    offset += 2;

            //    Console.WriteLine("Product Code: {0}", mBlock.DecodeShiftJISString(offset, 10));
            //    offset += 10;

            //    Console.WriteLine("Identifier: {0}", mBlock.DecodeShiftJISString(offset, 8));
            //    offset += 8;

            //    offset += 97; // unused bytes (length 97/0x61) "00"

            //    Console.WriteLine("Checksum: {0}", mBlock[offset++].ToString("X2"));

            //    Console.ReadKey();
            //}

            for (int i = 1; i <= 15; ++i)
            {
                byte[] block = mc.GetBlock(i);

                Console.WriteLine("(#{0}) Title Frame: ", i);
                Console.WriteLine("Magic values: {0}{1}", (char)(block[0]), (char)(block[1]));
                Console.WriteLine("Icon display flag: {0}", (int)block[2]);
                Console.WriteLine("Blocks used: {0}", (int)block[3]);
                Console.WriteLine("Title: {0}", shiftJisEncoding.GetString(block, 4, 64));
                Console.WriteLine(String.Empty);

                Console.ReadKey();
            }
        }
    }
}

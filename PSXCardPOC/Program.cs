using PSXMMCLibrary;
using System;
using System.IO;
using System.Text;

namespace PSXCardPOC
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryCard mc = new MemoryCard("ctr.mcr");
            Encoding shiftJisEncoding = Encoding.GetEncoding(932);

            byte[] mBlock = mc.GetHeaderBlock();

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

            for (int i = 1; i <= 15; ++i)
            {
                var block = mc.GetBlock(i);

                Console.WriteLine("(#{0}) Block: ", i);
                //Console.WriteLine("Magic values: {0}{1}", (char)(block[0]), (char)(block[1]));
                Console.WriteLine("Icon display flag: {0}", block.IconFrames);
                Console.WriteLine("Blocks used: {0}", block.BlocksUsed);
                Console.WriteLine("Title: {0}", block.Title);
                Console.WriteLine(String.Empty);

                Console.ReadKey();
            }
        }
    }
}

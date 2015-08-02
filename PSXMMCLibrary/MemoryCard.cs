using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSXMMCLibrary
{
    public class MemoryCard
    {
        private static readonly Encoding _shiftJisEncoding = Encoding.GetEncoding(932);
        private static readonly int _blockSize = 8192;

        private FileStream memCard;

        public MemoryCard(string filepath)
        {
            try
            {
                memCard = File.Open(filepath, FileMode.Open);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to open file.");
                Console.WriteLine(ex.ToString());
            }
        }

        public byte[] GetBlock(int index)
        {
            byte[] block = new byte[_blockSize];

            try
            {
                memCard.Seek(_blockSize * index, SeekOrigin.Begin);
                memCard.Read(block, 0, _blockSize);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to read block at index " + index);
                Console.WriteLine(ex.ToString());
            }

            return block;
        }

        public void ShowMagicBlock()
        {
            byte[] block = new byte[_blockSize];

            try
            {
                block = GetBlock(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to read header block.");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

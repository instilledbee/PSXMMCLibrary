using PSXMMCLibrary.Models;
using PSXMMCLibrary.Utilities;
using System;
using System.IO;
using System.Collections.Generic;

namespace PSXMMCLibrary
{
    public class MemoryCard
    {
        private FileStream _memCard;
        private List<DirectoryFrame> _directoryFrames;

        public MemoryCard(string filepath)
        {
            try
            {
                _memCard = File.Open(filepath, FileMode.Open);
                _directoryFrames = new List<DirectoryFrame>();
                ParseDirectoryFrames();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to open file.");
                Console.WriteLine(ex.ToString());
            }
        }

        public byte[] GetRawBlock(int index)
        {
            byte[] block = new byte[Constants.BlockLength];

            try
            {
                _memCard.Seek(Constants.BlockLength * index, SeekOrigin.Begin);
                _memCard.Read(block, 0, (int)Constants.BlockLength);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to read block at index " + index);
                Console.WriteLine(ex.ToString());
            }

            System.Diagnostics.Debug.WriteLine(block.ArrayToString());
            return block;
        }

        public Block GetBlock(int index)
        {
            byte[] block = GetRawBlock(index);
            Block parsedBlock = BlockParser.Parse(block);

            return parsedBlock;
        }

        public byte[] GetHeaderBlock()
        {
            byte[] block = new byte[Constants.BlockLength];

            try
            {
                block = GetRawBlock(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to read header block.");
                Console.WriteLine(ex.ToString());
            }

            return block;
        }

        public DirectoryFrame GetDirectoryFrame(int index)
        {
            if (_directoryFrames.Count == 0)
            {
                ParseDirectoryFrames();
            }

            return _directoryFrames[index];
        }

        private void ParseDirectoryFrames()
        {
            _directoryFrames.Clear();
            var headerBlock = GetHeaderBlock();

            for (int i = 0, offset = Constants.FrameLength; i < Constants.BlockCount; ++i, offset += Constants.FrameLength)
            {
                _directoryFrames.Add(DirectoryFrameParser.Parse(headerBlock.SubArray(offset, Constants.FrameLength)));
            }
        }
    }
}

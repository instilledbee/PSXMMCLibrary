using PSXMMCLibrary.Models;
using PSXMMCLibrary.Utilities;
using System;
using System.IO;
using System.Collections.Generic;

namespace PSXMMCLibrary
{
    public class MemoryCard
    {
        private static readonly int _BLOCK_SIZE = 8192;
        private static readonly int _FRAME_SIZE = 128;
        private static readonly int _BLOCK_COUNT = 15;

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

        public byte[] GetBlock(int index)
        {
            byte[] block = new byte[_BLOCK_SIZE];

            try
            {
                _memCard.Seek(_BLOCK_SIZE * index, SeekOrigin.Begin);
                _memCard.Read(block, 0, _BLOCK_SIZE);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unable to read block at index " + index);
                Console.WriteLine(ex.ToString());
            }

            return block;
        }

        public byte[] GetHeaderBlock()
        {
            byte[] block = new byte[_BLOCK_SIZE];

            try
            {
                block = GetBlock(0);
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

            for (int i = 0, offset = _FRAME_SIZE; i < _BLOCK_COUNT; ++i, offset += _FRAME_SIZE)
            {
                _directoryFrames.Add(DirectoryFrameParser.Parse(headerBlock.SubArray(offset, _FRAME_SIZE)));
            }
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSXMMCLibrary.Models;
using PSXMMCLibrary.Models.Enums;

namespace PSXMMCLibrary.Tests
{
    [TestClass]
    public class DirectoryFrameParserTests
    {
        private byte[] validData = new byte[] { 
            81, 0, 0, 0, 0, 32, 0, 0, 255, 255, 66, 65, 83, 67, 85, 83, 45, 
            57, 52, 52, 50, 54, 45, 83, 76, 79, 84, 83, 0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
            0, 0, 0, 0, 14 
        };

        private byte[] emptyData = new byte[128];

        [TestMethod]
        public void DirectoryFrameParser_ParseValidData()
        {
            // Arrange
            byte[] data = validData;
            DirectoryFrame outputFrame;

            // Act
            outputFrame = DirectoryFrameParser.Parse(data);

            // Assert
            Assert.IsNotNull(outputFrame);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DirectoryFrameParser_ParseEmptyData()
        {
            // Arrange
            byte[] data = new byte[] { };
            DirectoryFrame outputFrame;

            // Act
            outputFrame = DirectoryFrameParser.Parse(data);

            // Assert
            //Assert.IsNull(outputFrame);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DirectoryFrameParser_ParseNullData()
        {
            // Arrange
            byte[] data = null;
            DirectoryFrame outputFrame;

            // Act
            outputFrame = DirectoryFrameParser.Parse(data);

            // Assert
            //Assert.IsNull(outputFrame);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void DirectoryFrameParser_ParseInvalidAvailableFlag()
        {
            // Arrange
            byte[] data = emptyData;
            data[0] = 1;
            DirectoryFrame outputFrame;

            // Act
            outputFrame = DirectoryFrameParser.Parse(data);

            // Assert
            //Assert.IsNull(outputFrame);
        }

        [TestMethod]
        public void DirectoryFrameParser_ParseValidAvailableFlag()
        {
            // Arrange
            byte[] data = validData;
            DirectoryFrame outputFrame;

            // Act
            outputFrame = DirectoryFrameParser.Parse(data);

            // Assert
            Assert.AreEqual(AvailableStatus.FirstLink, outputFrame.AvailableStatus);
        }

        // TODO: Determine invalidity of blocks used bytes
        [TestMethod]
        [Ignore]
        public void DirectoryFrameParser_ParseInvalidBlocksUsed()
        {
            // Arrange
            byte[] data = emptyData;
            DirectoryFrame outputFrame;

            // Act
            outputFrame = DirectoryFrameParser.Parse(data);

            // Assert
            //Assert.AreEqual(AvailableStatus.FirstLink, outputFrame.AvailableStatus);
        }

        [TestMethod]
        public void DirectoryFrameParser_ParseValidBlocksUsed()
        {
            // Arrange
            byte[] data = validData;
            DirectoryFrame outputFrame;

            // Act
            outputFrame = DirectoryFrameParser.Parse(data);

            // Assert
            Assert.AreEqual(1, outputFrame.BlocksUsed);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void DirectoryFrameParser_ParseInvalidLinkOrder()
        {
            // Arrange
            byte[] data = emptyData;
            data[8] = 2;
            data[9] = 2;
            DirectoryFrame outputFrame;

            // Act
            outputFrame = DirectoryFrameParser.Parse(data);

            // Assert
            //Assert.IsNull(outputFrame);
        }

        [TestMethod]
        public void DirectoryFrameParser_ParseValidLinkOrder()
        {
            // Arrange
            byte[] data = validData;
            DirectoryFrame outputFrame;

            // Act
            outputFrame = DirectoryFrameParser.Parse(data);

            // Assert
            Assert.AreEqual(-1, outputFrame.LinkOrder);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DirectoryFrameParser_ParseInvalidCountryCode()
        {
            // Arrange
            byte[] data = validData;
            data[10] = 1;
            data[11] = 1;
            DirectoryFrame outputFrame;

            // Act
            outputFrame = DirectoryFrameParser.Parse(data);

            // Assert
            //Assert.IsNull(outputFrame);
        }

        [TestMethod]
        public void DirectoryFrameParser_ParseValidIdentifier()
        {
            // Arrange
            byte[] data = validData;
            DirectoryFrame outputFrame;

            // Act
            outputFrame = DirectoryFrameParser.Parse(data);

            // Assert
            Assert.AreEqual("-SLOTS\0\0", outputFrame.Identifier);
        }

        [TestMethod]
        public void DirectoryFrameParser_ParseValidProductCode()
        {
            // Arrange
            byte[] data = validData;
            DirectoryFrame outputFrame;

            // Act
            outputFrame = DirectoryFrameParser.Parse(data);

            // Assert
            Assert.AreEqual("SCUS-94426", outputFrame.ProductCode);
        }
    }
}

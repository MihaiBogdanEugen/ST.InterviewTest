using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STInterviewTest.Infrastructure.TextExtractors;

namespace STInterviewTest.Infrastructure.Tests
{
    [TestClass]
    public class PdfFileTextExtractorTest
    {
        private readonly PdfFileTextExtractor _textExtractor;

        public PdfFileTextExtractorTest()
        {
            this._textExtractor = new PdfFileTextExtractor();
        }

        [TestMethod]
        public void Test_ExtractText_Ok()
        {
            var expectedResult = File.ReadAllText(@"Files\sample1.txt").RemoveNewLineAndLineFeed();

            var actualResult = this._textExtractor.ExtractText(@"Files\sample1.pdf");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Test_ExtractText_Ok2()
        {
            var expectedResult = File.ReadAllText(@"Files\sample2.txt").RemoveNewLineAndLineFeed();

            var actualResult = this._textExtractor.ExtractText(@"Files\sample2.pdf");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void Test_ExtractText_Ok3()
        {
            var expectedResult = File.ReadAllText(@"Files\sample3.txt").RemoveNewLineAndLineFeed();

            var actualResult = this._textExtractor.ExtractText(@"Files\sample3.pdf");

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value cannot be null")]
        public void Test_ExtractText_NotOk_EmptyFilePath()
        {
            this._textExtractor.ExtractText(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException), "Cannot find file [invalid file path]")]
        public void Test_ExtractText_NotOk_WrongFilePath()
        {
            this._textExtractor.ExtractText("invalid file path");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), @"File [Files\sample1.txt] cannot be processed by a PdfFileTextExtractor")]
        public void Test_ExtractText_NotOk_WrongExtension()
        {
            this._textExtractor.ExtractText(@"Files\sample1.txt");
        }
    }
}

using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STInterviewTest.Infrastructure.PreProcessors;

namespace STInterviewTest.Infrastructure.Tests
{
    [TestClass]
    public class UnzipperPreProcessorTest
    {
        [TestMethod]
        public void Test_Unzip_Ok1()
        {
            if (File.Exists(@"Files\sample12.txt"))
                File.Delete(@"Files\sample12.txt");

            var filePath = UnzipperPreProcessor.UnzipFile(@"Files\sample1.txt.zip");

            Assert.IsNotNull(filePath);
            Assert.IsTrue(File.Exists(filePath));
            Assert.AreEqual(File.ReadAllText(@"Files\sample1.txt"), File.ReadAllText(@"Files\sample12.txt"));
        }

//        [TestMethod]
//        public void Test_Unzip_Ok2()
//        {
//            if (File.Exists(@"Files\sample12.pdf"))
//                File.Delete(@"Files\sample12.pdf");
//
//            var filePath = UnzipperPreProcessor.UnzipFile(@"Files\sample1.pdf.zip");
//
//            Assert.IsNotNull(filePath);
//            Assert.IsTrue(File.Exists(filePath));
//            Assert.AreEqual(File.ReadAllText(@"Files\sample1.pdf"), File.ReadAllText(@"Files\sample12.pdf"));
//        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace STInterviewTest.Infrastructure.Tests
{
    [TestClass]
    public class PipelineTest
    {
        private readonly Pipeline _pipeline;

        public PipelineTest()
        {
            this._pipeline = Pipeline.Setup();
        }

        [TestMethod]
        public void IntegrationTest_Process_Txt_Ok1()
        {
            var actualResult = this._pipeline.ProcessTxtFile(@"Files\sample1.txt");

            Assert.AreEqual("Positive Opinion", actualResult);
        }

        [TestMethod]
        public void IntegrationTest_Process_Txt_Ok2()
        {
            var actualResult = this._pipeline.ProcessTxtFile(@"Files\sample2.txt");

            Assert.AreEqual("Positive Opinion", actualResult);
        }

        [TestMethod]
        public void IntegrationTest_Process_Txt_Ok3()
        {
            var actualResult = this._pipeline.ProcessTxtFile(@"Files\sample3.txt");

            Assert.AreEqual("Positive Opinion", actualResult);
        }

        [TestMethod]
        public void IntegrationTest_Process_Txt_Ok4()
        {
            var actualResult = this._pipeline.ProcessTxtFile(@"Files\sample4.txt");

            Assert.AreEqual("Neutral Opinion", actualResult);
        }

        [TestMethod]
        public void IntegrationTest_Process_Txt_Ok5()
        {
            var actualResult = this._pipeline.ProcessTxtFile(@"Files\sample5.txt");

            Assert.AreEqual("Negative Opinion", actualResult);
        }

        [TestMethod]
        public void IntegrationTest_Process_Pdf_Ok1()
        {
            var actualResult = this._pipeline.ProcessPdfFile(@"Files\sample1.pdf");

            Assert.AreEqual("Positive Opinion", actualResult);
        }

        [TestMethod]
        public void IntegrationTest_Process_Pdf_Ok2()
        {
            var actualResult = this._pipeline.ProcessPdfFile(@"Files\sample2.pdf");

            Assert.AreEqual("Positive Opinion", actualResult);
        }

        [TestMethod]
        public void IntegrationTest_Process_Pdf_Ok3()
        {
            var actualResult = this._pipeline.ProcessPdfFile(@"Files\sample3.pdf");

            Assert.AreEqual("Positive Opinion", actualResult);
        }

        [TestMethod]
        public void IntegrationTest_Process_Pdf_Ok4()
        {
            var actualResult = this._pipeline.ProcessPdfFile(@"Files\sample4.pdf");

            Assert.AreEqual("Neutral Opinion", actualResult);
        }

        [TestMethod]
        public void IntegrationTest_Process_Pdf_Ok5()
        {
            var actualResult = this._pipeline.ProcessPdfFile(@"Files\sample5.pdf");

            Assert.AreEqual("Negative Opinion", actualResult);
        }

        [TestMethod]
        public void IntegrationTest_Process_ZipTxt_Ok1()
        {
            var actualResult = this._pipeline.ProcessTxtFile(@"Files\sample1.txt.zip");

            Assert.AreEqual("Positive Opinion", actualResult);
        }

        [TestMethod]
        public void IntegrationTest_Process_ZipPdf_Ok1()
        {
            var actualResult = this._pipeline.ProcessPdfFile(@"Files\sample1.pdf.zip");

            Assert.AreEqual("Positive Opinion", actualResult);
        }
    }
}

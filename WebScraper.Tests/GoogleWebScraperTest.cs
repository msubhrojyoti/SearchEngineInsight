using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace WebScraper.Tests
{
    [TestClass]
    public class GoogleWebScraperTest
    {
        private Mock<IHttpHandler> _mockHttpHandler;
        private IHttpHandler _httpHandler;

        /// <summary>
        ///     Test setup.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _mockHttpHandler = new Mock<IHttpHandler>();
            _httpHandler = new HttpHandler();
        }

        /// <summary>
        ///     Test clean up.
        /// </summary>
        [TestCleanup]
        public void Teardown()
        {
        }

        [TestMethod]
        [TestCategory("Unit")]
        [DeploymentItem("TestData\\TestScrapeData.xml", "TestData\\TestScrapeData.xml")]
        public void TestMockScrape()
        {
            string mockResponse = File.ReadAllText("TestData\\TestScrapeData.xml");
            _mockHttpHandler.Setup(
                x => x.ExecuteHttpGet(
                    "https://google.com.au",
                    "/search",
                    It.IsAny<List<Tuple<string, object, ParameterType>>>(),
                    out mockResponse)).Returns(HttpStatusCode.OK);

            GoogleWebScraper scraper = new GoogleWebScraper(_mockHttpHandler.Object);
            var result = scraper.Scrape("https://google.com.au", "abc", 100);
            Assert.AreEqual(mockResponse, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TestMockScrapeWithError()
        {
            string mockResponse = "sample response";
            _mockHttpHandler.Setup(
                x => x.ExecuteHttpGet(
                    "https://google.com.au",
                    "/search",
                    It.IsAny<List<Tuple<string, object, ParameterType>>>(),
                    out mockResponse)).Returns(HttpStatusCode.NotFound);

            GoogleWebScraper scraper = new GoogleWebScraper(_mockHttpHandler.Object);
            Assert.ThrowsException<HttpRequestException>(()=> scraper.Scrape("https://google.com.au", "abc", 100));
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TestScrape()
        {
            GoogleWebScraper scraper = new GoogleWebScraper(_httpHandler);
            string server = "https://google.com.au";
            string keywords = "conveyancing software";

            var resposne = scraper.Scrape(server, keywords, 100);

            Assert.IsFalse(string.IsNullOrEmpty(resposne));
            Assert.IsTrue(resposne.Length > 1000);
            Assert.IsTrue((new string[] { "Conveyancing", "smokeball.com.au" }).All(x => resposne.Contains(x)));
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebScraper;

namespace SearchEngineInsightApp.Tests
{
    [TestClass]
    public class GoogleScrapeWrapperTest
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void TestStartScraping()
        {
            Mock<IHttpHandler> handler = new Mock<IHttpHandler>();
            GoogleScrapeWrapper w = new GoogleScrapeWrapper(handler.Object);
            string response = "<div class=\"kCrYT\"><a href=\"/url?q=https://www.leap.com.au/area-of-law/conveyancing/&amp;sa=U&amp;ved=2ahUKEwjJy6myjtbyAhX1rJUCHei-DxYQFnoECGEQAQ&amp;usg=AOvVaw35_dKIv7AdBCgEfSAK-GOe\"><h3 class=\"zBAuLc\"><div class=\"BNeawe vvjwJb AP7Wnd\">Practice Management Software for Conveyancers | LEAP Legal ...</div></h3><div class=\"BNeawe UPmit AP7Wnd\">www.leap.com.au &#8250; area-of-law &#8250; conveyancing</div></a></div>";
            handler.Setup(
               x => x.ExecuteHttpGet(
                   It.IsAny<string>(),
                   It.IsAny<string>(),
                   It.IsAny<List<Tuple<string, object, ParameterType>>>(),
                   out response)).Returns(HttpStatusCode.OK);

            var results = w.StartScraping("x","y",100,"missingitem", out string[] ranks);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0].Contains("leap.com.au"));
            Assert.IsTrue(ranks.Length == 0);

            results = w.StartScraping("x", "y", 100, "leap.com.au", out ranks);
            Assert.IsTrue(results.Count == 1);
            Assert.IsTrue(results[0].Contains("leap.com.au"));
            Assert.IsTrue(ranks.Length == 1);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TestStartScrapingIntegration()
        {
            GoogleScrapeWrapper w = new GoogleScrapeWrapper(new HttpHandler());
            var results = w.StartScraping("https://google.com.au", "conveyancing software", 100, "smokeball.com.au", out string[] ranks);
            Assert.IsTrue(results.Count == 100);
            Assert.IsTrue(results.Any(x => x.Contains("smokeball.com.au")));
            Assert.IsTrue(ranks.Length != 0);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TestStartScrapingIntegrationWithNewSearch()
        {
            GoogleScrapeWrapper w = new GoogleScrapeWrapper(new HttpHandler());
            var results = w.StartScraping("https://google.com.au", "house", 100, "realestate.com.au", out string[] ranks);
            Assert.IsTrue(results.Count > 10);
            Assert.IsTrue(results.Any(x => x.Contains("realestate.com.au")));
            Assert.IsTrue(ranks.Length != 0);
        }
    }
}

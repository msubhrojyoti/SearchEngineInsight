using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace HtmlParser.Tests
{
    [TestClass]
    public class XmlParserUtilityTest
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void TestFindOccurences()
        {
            string input = "This is a test string. And this test string is present in a test string";
            int[] expected = new int[] { 10, 32, 60};
            foreach (var x in XmlParserUtility.FindOccurences(input, "test string"))
            {
                Assert.IsTrue(expected.Contains(x));
            }

            int[] actual = XmlParserUtility.FindOccurences(input, "test string").ToArray();
            Assert.IsTrue(actual.SequenceEqual(expected), "Sequences should be same");

            string inputx = "sub1 susu";
            int[] expectedx = new int[] { 0, 5, 7 };
            foreach (var x in XmlParserUtility.FindOccurences(inputx, "su"))
            {
               //Assert.IsTrue(expectedx.Contains(x));
            }

            int[] actualx = XmlParserUtility.FindOccurences(inputx, "su").ToArray();
            Assert.IsTrue(actualx.SequenceEqual(expectedx), "Sequences should be same");
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TestFindOccurencesWithInvalidData()
        {
            string input = "This is a test string. And this test string is present in a test string";
            Assert.IsTrue(XmlParserUtility.FindOccurences(input, "not found").ToArray().Count() == 0);
            Assert.ThrowsException<ArgumentException>(() => XmlParserUtility.FindOccurences(string.Empty, "test string").ToArray());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TestGetFirstXmlNodeValue()
        {
            string input = "<div><div class=\"someclass\">some data</div></div>";
            Assert.AreEqual("some data", XmlParserUtility.GetFirstXmlNodeValue(input, "div", "class", "someclass"));

            input = "<div><div class=\"someclass\">some data</div></div>";
            Assert.ThrowsException<ArgumentException>(() => XmlParserUtility.GetFirstXmlNodeValue(input, "div", "class", "abc"));

            input = "<div><div class=\"someclass\">some data</div><div class=\"someclass\">some more data</div></div>";
            Assert.AreEqual("some data", XmlParserUtility.GetFirstXmlNodeValue(input, "div", "class", "someclass"));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TestParseFirstXmlNode()
        {
            string input = "<div><div class=\"someclass\">some data</div></div>";
            Assert.AreEqual(input, XmlParserUtility.ParseFirstCompleteXmlNode(input, 0));
            Assert.AreEqual("<div class=\"someclass\">some data</div>", XmlParserUtility.ParseFirstCompleteXmlNode(input, 5));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TestParseFirstXmlNodeWithSelfClosingTag()
        {
            string inputWithSelfClosingTag = "<div><br/><div class=\"someclass\">some data</div></div>";
            Assert.AreEqual(inputWithSelfClosingTag, XmlParserUtility.ParseFirstCompleteXmlNode(inputWithSelfClosingTag, 0));
            Assert.AreEqual("<br/>", XmlParserUtility.ParseFirstCompleteXmlNode(inputWithSelfClosingTag, 5));
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void TestParseFirstXmlNodeWithInvalidInput()
        {
            Assert.ThrowsException<ArgumentException>(() => XmlParserUtility.ParseFirstCompleteXmlNode(string.Empty, 0));
            Assert.ThrowsException<ArgumentException>(() => XmlParserUtility.ParseFirstCompleteXmlNode("abc", -5));
        }
    }
}

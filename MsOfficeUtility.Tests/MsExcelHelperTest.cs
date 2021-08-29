using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MsOfficeUtility.Tests
{
    [TestClass]
    public class MsExcelHelperTest
    {
        [DeploymentItem("TestData\\InputExcel.xlsx", "TestData")]
        [TestMethod]
        [TestCategory("Unit")]
        public void TestReadDataAccuracy()
        {
            string inputFile = @"TestData\InputExcel.xlsx";
            int expectedDataCount = 3;
            var results = MsExcelHelper.ReadMsExcelData(inputFile, "SheetJS").Take(expectedDataCount);
            Assert.AreEqual(expectedDataCount, results.Count(), "Expected data count should match");

            List<Tuple<string, string>> inputData = new(expectedDataCount)
            {
                Tuple.Create("URL_clean", "Title"),
                Tuple.Create("https://www.leapconveyancer.com.au/", "LEAP Conveyancer: Conveyancing Practice Management ..."),
                Tuple.Create("https://www.intouch.cloud/", "Conveyancing Software, Case Management Software | InTouch"),
            };

            for(int i=0; i < expectedDataCount; i++)
            {
                Assert.AreEqual(inputData[i].Item1, results.ElementAt(i).ElementAt(0));
                Assert.AreEqual(inputData[i].Item2, results.ElementAt(i).ElementAt(1));
            }
        }

        [DeploymentItem("TestData\\InputExcel.xlsx", "TestData")]
        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(ArgumentException), "Worksheet 'ABC' does not exist in file 'TestData\\InputExcel.xlsx'.")]
        public void TestInvalidWorksheetName()
        {
            string inputFile = @"TestData\InputExcel.xlsx";
            MsExcelHelper.ReadMsExcelData(inputFile, "ABC").ToArray();
        }

        [DeploymentItem("TestData\\InputExcel.xlsx", "TestData")]
        [TestMethod]
        [TestCategory("Unit")]
        public void TestReadAllData()
        {
            string inputFile = @"TestData\InputExcel.xlsx";
            string key = "smokeball.com.au";
            short expectedCount = 1;
            short expectedAllDataCount = 105;

            var fullData = MsExcelHelper.ReadMsExcelData(inputFile, "SheetJS").ToArray();
            Assert.IsTrue(fullData.Length == expectedAllDataCount, $"Entire data count should be {expectedAllDataCount}");
            var actualCount = fullData.Count(x => x.Any(x => x.Contains(key)));
            Assert.AreEqual(expectedCount, actualCount, $"Count of '{key}' should be {expectedCount}");
        }
    }
}

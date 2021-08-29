using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MsOfficeUtility
{
    public class MsExcelHelper
    {
        /// <summary>
        ///     Reads the excel worksheet cell values as an array of string arrays.
        /// </summary>
        /// <param name="fileName">name of the excel file</param>
        /// <param name="sheetName">name of the excel worksheet</param>
        /// <returns>
        ///     Array of string arrays. Each array element is an array representing the rows of the excel worksheet.
        /// </returns>
        public static IEnumerable<string[]> ReadMsExcelData(string fileName, string sheetName)
        {
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fileName, false))
            {
                Sheet sheet = doc.WorkbookPart.Workbook.Descendants<Sheet>().SingleOrDefault(
                    s => s.Name.ToString().Equals(sheetName, StringComparison.CurrentCultureIgnoreCase));

                if (sheet == null)
                {
                    throw new ArgumentException(
                        $"Worksheet '{sheetName}' does not exist in file '{fileName}'.");
                }

                WorksheetPart worksheetPart = (WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id);
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                int noOfColumns = 0;
                foreach (var row in sheetData.Elements<Row>())
                {
                    noOfColumns = row.Elements().Count();
                    string[] cellValues = new string[noOfColumns];
                    for (int colIndex = 0; colIndex < noOfColumns; colIndex++)
                    {
                        cellValues[colIndex] = GetCellValue(doc, row.Descendants<Cell>().ElementAt(colIndex));
                    }

                    yield return cellValues;
                }
            }
        }

        /// <summary>
        ///     Gets cell value as string
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cell"></param>
        /// <returns>value of the cell</returns>
        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            if (cell?.CellValue == null)
            {
                return string.Empty;
            }

            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml.Trim();

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[int.Parse(value)].InnerText.Trim();
            }

            return value;
        }
    }
}

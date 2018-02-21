using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;

namespace MyPS6Tests
{
    [TestClass]
    public class PS6Tests
    {
        /// <summary>
        /// Check if GetCellValue returns a double
        /// </summary>
        [TestMethod]
        public void GetCellValue()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            Assert.AreEqual(5.0, sheet.GetCellValue("A1"));
        }

        /// <summary>
        /// Check if GetCellValue catches FormulaEvaluationException
        /// and returns a FormulaError object.
        /// </summary>
        [TestMethod]
        public void GetCellValue2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=C");
            Assert.AreEqual(new FormulaError(), sheet.GetCellValue("A1"));
        }

        /// <summary>
        /// Check if GetCellValue catches FormulaEvaluationException
        /// and returns a FormulaError object.
        /// </summary>
        [TestMethod]
        public void Save()
        {
            StringWriter s = new StringWriter();
            s.Write("../../spreadsheet1.xml");
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "ok");
            sheet.Save(s);
        }
    }
}

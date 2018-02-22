using System;
using System.IO;
using Formulas;
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
        /// Check if GetCellValue throws FormulaFormatException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void GetCellValue2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=?");
        }

        /// <summary>
        /// Check if GetCellValue catches FormulaEvaluationException
        /// and returns a FormulaError object.
        /// </summary>
        [TestMethod]
        public void GetCellValue3()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=x5");
            Assert.IsTrue(sheet.GetCellValue("A1").GetType() == typeof(FormulaError));
        }

        /// <summary>
        /// Check if GetCellValue evaluates a valid formula
        /// </summary>
        [TestMethod]
        public void GetCellValue4()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=10+4");
            Assert.AreEqual(14.0, sheet.GetCellValue("A1"));
        }
        /// <summary>
        /// Check if GetCellValue returns string
        /// </summary>
        [TestMethod]
        public void GetCellValue5()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "parameter");
            Assert.AreEqual("parameter", sheet.GetCellValue("A1"));
        }

        /// <summary>
        /// Check to see if Save works and saves XML document.
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

using System;
using Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;

namespace UnitTestProject2
{
    [TestClass]
    public class Spreadsheet2Tests
    {
        /// <summary>
        /// Check if GetCellValue catches FormulaEvaluationException
        /// and returns a FormulaError object.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void GetCellValue()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", new Formula("5/0").ToString());
            sheet.GetCellValue("A1");
        }
    }
}

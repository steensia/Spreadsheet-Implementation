using System;
using System.Collections.Generic;
using Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// Check if GetCellContents throws an InvalidNameException when name is null or invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsNullCheck()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents(null);
        }

        /// <summary>
        /// Check if SetCellContents throws an InvalidNameException when name is null or invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsNullCheck()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents(null, 20);
        }

        /// <summary>
        /// Check if SetCellContents throws an ArgumentNullException when text is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellContentsNullCheck2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("x", null);
        }

        /// <summary>
        /// Check if SetCellContents throws an InvalidNameException when name is null or invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsNullCheck3()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents(null, "x");
        }
    }
}

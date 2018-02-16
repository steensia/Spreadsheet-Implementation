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
        /// Check if GetCellContents returns the correct string
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents1()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", "xD");
            Assert.AreEqual("xD", sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Check if GetCellContents returns the correct double
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 7);
            Assert.AreEqual(7, sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Check if GetCellContents returns the correct formula with default constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents3()
        {
            Spreadsheet sheet = new Spreadsheet();
            Formula defConstruct = new Formula();
            sheet.SetCellContents("A1", defConstruct);
            Assert.AreEqual("", sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Check if GetCellContents returns the correct formula with old constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents4()
        {
            Spreadsheet sheet = new Spreadsheet();
            Formula defConstruct = new Formula("X + Y");
            sheet.SetCellContents("A1", defConstruct);
            Assert.AreEqual("X+Y", sheet.GetCellContents("A1"));
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

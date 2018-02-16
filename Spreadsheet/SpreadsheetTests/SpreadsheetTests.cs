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
        public void GetCellContents2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 7.0);
            Assert.AreEqual(7.0, sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Check if GetCellContents returns the correct formula with default constructor
        /// </summary>
        [TestMethod]
        public void GetCellContents3()
        {
            Spreadsheet sheet = new Spreadsheet();
            Formula defConstruct = new Formula();
            sheet.SetCellContents("A1", defConstruct.ToString());
            Assert.AreEqual("0", sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Check if GetCellContents returns the correct formula with old constructor
        /// </summary>
        [TestMethod]
        public void GetCellContents4()
        {
            Spreadsheet sheet = new Spreadsheet();
            Formula defConstruct = new Formula("X + Y");
            sheet.SetCellContents("A1", defConstruct.ToString());
            Assert.AreEqual("X+Y", sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Check GetCellContents on no existing cell
        /// </summary>
        [TestMethod]
        public void GetCellContents5()
        {
            Spreadsheet sheet = new Spreadsheet();
            Formula defConstruct = new Formula("X + Y");
            sheet.SetCellContents("A1", defConstruct.ToString());
            Assert.AreEqual("", sheet.GetCellContents("B1"));
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

        /// <summary>
        /// Check if for CircularException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularException1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("B2"));
            s.SetCellContents("B2", new Formula("A1+B3"));
        }

        /// <summary>
        /// Check if SetCellContents creates a new cell with string content
        /// </summary>
        [TestMethod]
        public void SetCellContentsString()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A7", "ok");
            Assert.AreEqual("ok", sheet.GetCellContents("A7"));
        }

        /// <summary>
        /// Check if SetCellContents replaces string content of existing cell 
        /// </summary>
        [TestMethod]
        public void SetCellContentsString2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A7", "ok");
            sheet.SetCellContents("A7", "");
            Assert.AreEqual("ok", sheet.GetCellContents("A7"));
        }
    }
}

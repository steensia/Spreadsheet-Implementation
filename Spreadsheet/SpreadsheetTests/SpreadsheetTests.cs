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
        /// Check if all valid cells are returned
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("C2", "3");
            s.SetContentsOfCell("C2", "");
            HashSet<string> cellNames = new HashSet<string>(s.GetNamesOfAllNonemptyCells());
            Assert.IsTrue(cellNames.SetEquals(new HashSet<string>()));
        }

        /// <summary>
        /// Check if all valid cells are returned
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "5");
            s.SetContentsOfCell("B2", "x2");
            s.SetContentsOfCell("C3", "=D7");
            s.SetContentsOfCell("D4", "=F9");
            HashSet<string> cellNames = new HashSet<string>(s.GetNamesOfAllNonemptyCells());
            Assert.IsTrue(cellNames.SetEquals(new HashSet<string> { "A1", "B2", "C3", "D4" }));
        }

        /// <summary>
        /// Check if all valid cells are returned
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B3");
            s.SetContentsOfCell("B2", "=C5");
            s.SetContentsOfCell("C3", "=D7");
            s.SetContentsOfCell("D4", "=F9");
            s.SetContentsOfCell("D4", "");
            HashSet<string> cellNames = new HashSet<string>(s.GetNamesOfAllNonemptyCells());
            Assert.IsTrue(cellNames.SetEquals(new HashSet<string> { "A1", "B2", "C3" }));
        }

        /// <summary>
        /// Check if all valid cells are returned, stress test
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells3()
        {
            Spreadsheet s = new Spreadsheet();
            HashSet<string> t = new HashSet<string>();
            for (int i = 0; i < 1_000_000; i++)
            {
                s.SetContentsOfCell("A1" + i, "B2" + i);
                t.Add("A1" + i);
            }
            for (int i = 777; i < 777_777; i++)
            {
                t.Remove("A1" + i);
                s.SetContentsOfCell("A1" + i, "");
            }
            HashSet<string> cellNames = new HashSet<string>(s.GetNamesOfAllNonemptyCells());
            Assert.IsTrue(cellNames.SetEquals(t));
        }

        /// <summary>
        /// Check if GetCellContents returns the correct string
        /// </summary>
        [TestMethod]
        public void GetCellContents1()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "xD");
            Assert.AreEqual("xD", sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Check if GetCellContents returns the correct double
        /// </summary>
        [TestMethod]
        public void GetCellContents2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "7.0");
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
            sheet.SetContentsOfCell("A1", defConstruct.ToString());
            Assert.AreEqual(0.0, sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Check if GetCellContents returns the correct formula with old constructor
        /// </summary>
        [TestMethod]
        public void GetCellContents4()
        {
            Spreadsheet sheet = new Spreadsheet();
            Formula defConstruct = new Formula("X + Y");
            sheet.SetContentsOfCell("A1", defConstruct.ToString());
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
            sheet.SetContentsOfCell("A1", defConstruct.ToString());
            Assert.AreEqual("", sheet.GetCellContents("B1"));
        }

        /// <summary>
        /// Check if SetContentsOfCell throws an InvalidNameException when name is invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellInvalidName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("mm", "20");
        }

        /// <summary>
        /// Check if SetContentsOfCell throws an InvalidNameException when name is null 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellNullCheck()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "20");
        }

        /// <summary>
        /// Check if SetContentsOfCell throws an ArgumentNullException when text is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetContentsOfCellNullCheck2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("x", null);
        }

        /// <summary>
        /// Check if SetContentsOfCell throws an InvalidNameException when name is null or invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellNullCheck3()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "x");
        }

        /// <summary>
        /// Check if CircularException is thrown
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularException1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B2");
            s.SetContentsOfCell("B2", "=A1+B3");
        }


        /// <summary>
        /// Check if for CircularException is caught and previous state is maintained
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularException2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B2");
            s.SetContentsOfCell("B2", "=B3+4");
            s.SetContentsOfCell("B2", "=A1");
        }

        /// <summary>
        /// Check if SetContentsOfCell creates a new cell with string content
        /// </summary>
        [TestMethod]
        public void SetContentsOfCellString()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A7", "ok");
            Assert.AreEqual("ok", sheet.GetCellContents("A7"));
        }

        /// <summary>
        /// Check if SetContentsOfCell replaces string content of existing cell 
        /// </summary>
        [TestMethod]
        public void SetContentsOfCellString2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A7", "ok");
            sheet.SetContentsOfCell("A7", "cow");
            sheet.SetContentsOfCell("A7", "xD");
            Assert.AreEqual("xD", sheet.GetCellContents("A7"));
        }

        /// <summary>
        /// Check if SetContentsOfCell replaces string content of existing cell with empty string
        /// </summary>
        [TestMethod]
        public void SetContentsOfCellString3()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A7", "ok");
            sheet.SetContentsOfCell("A7", "");
            Assert.AreEqual("", sheet.GetCellContents("A7"));
        }

        /// <summary>
        /// Check if SetContentsOfCell replaces string content of existing cell 
        /// </summary>
        [TestMethod]
        public void SetContentsOfCellString4()
        {
            Spreadsheet sheet = new Spreadsheet();
            Formula f = new Formula();
            sheet.SetContentsOfCell("A7", f.ToString());
            sheet.SetContentsOfCell("A1", "a");
            sheet.SetContentsOfCell("A7", "");
            Assert.AreEqual("", sheet.GetCellContents("A7"));
        }

        /// <summary>
        /// Check if SetContentsOfCell puts a double in cell content
        /// </summary>
        [TestMethod]
        public void SetContentsOfCellDouble1()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "10");
            Assert.AreEqual(10.0, sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Check if SetContentsOfCell replaces double content of existing cell 
        /// </summary>
        [TestMethod]
        public void SetContentsOfCellDouble2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A7", "5");
            sheet.SetContentsOfCell("A1", "10");
            sheet.SetContentsOfCell("A7", "3");
            Assert.AreEqual(3.0, sheet.GetCellContents("A7"));
        }

        /// <summary>
        /// Check if SetContentsOfCell formula evaluates to correct answer
        /// </summary>
        [TestMethod]
        public void SetContentsOfCellFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            Formula f = new Formula();
            sheet.SetContentsOfCell("A1", f.ToString());
            Assert.AreEqual(0.0, f.Evaluate(s => 0));
        }

        ///// <summary>
        ///// Check if SetContentsOfCell formula evaluates to correct answer, stress test 1
        ///// </summary>
        //[TestMethod]
        //public void SetContentsOfCellFormula1()
        //{
        //    Spreadsheet sheet = new Spreadsheet();
        //    for (int i = 1; i <= 100; i++)
        //    {
        //        sheet.SetContentsOfCell("x" + i, "=A+" + i * 2));
        //        Formula temp = (Formula) (sheet.GetCellContents("x" + i));
        //        Assert.AreEqual(i * 2, temp.Evaluate( x => 0));
        //    }

        //}

        ///// <summary>
        ///// Check if SetContentsOfCell formula evaluates to correct answer, stress test 2
        ///// </summary>
        //[TestMethod]
        //public void SetContentsOfCellFormula2()
        //{
        //    Spreadsheet sheet = new Spreadsheet();
        //    for (int i = 1; i <= 1_000; i++)
        //    {
        //        sheet.SetContentsOfCell("A" + i + i, "=A+" + i * 2 * i + "/ 2"));
        //        Formula temp = (Formula)(sheet.GetCellContents("A" + i + i));
        //        Assert.AreEqual(3 + i * 2 * i / 2, temp.Evaluate(x => 3));
        //    }

        //}
    }
}

        



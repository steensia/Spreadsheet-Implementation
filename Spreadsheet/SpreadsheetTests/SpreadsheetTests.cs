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
            s.SetCellContents("C2", 3);
            s.SetCellContents("C2", "");
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
            s.SetCellContents("A1", 5);
            s.SetCellContents("B2", "x2");
            s.SetCellContents("C3", new Formula("D7"));
            s.SetCellContents("D4", new Formula("F9"));
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
            s.SetCellContents("A1", new Formula("B3"));
            s.SetCellContents("B2", new Formula("C5"));
            s.SetCellContents("C3", new Formula("D7"));
            s.SetCellContents("D4", new Formula("F9"));
            s.SetCellContents("D4", "");
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
                s.SetCellContents("A1" + i, "B2" + i);
                t.Add("A1" + i);
            }
            for (int i = 777; i < 777_777; i++)
            {
                t.Remove("A1" + i);
                s.SetCellContents("A1" + i, "");
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
        /// Check if SetCellContents throws an InvalidNameException when name is invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsInvalidName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("mm", 20);
        }

        /// <summary>
        /// Check if SetCellContents throws an InvalidNameException when name is null 
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
        /// Check if CircularException is thrown
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
        /// Check if for CircularException is caught and previous state is maintained
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void CircularException2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("B2"));
            s.SetCellContents("B2", new Formula("B3+4"));
            s.SetCellContents("B2", new Formula("A1"));
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
            sheet.SetCellContents("A7", "cow");
            sheet.SetCellContents("A7", "xD");
            Assert.AreEqual("xD", sheet.GetCellContents("A7"));
        }

        /// <summary>
        /// Check if SetCellContents replaces string content of existing cell with empty string
        /// </summary>
        [TestMethod]
        public void SetCellContentsString3()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A7", "ok");
            sheet.SetCellContents("A7", "");
            Assert.AreEqual("", sheet.GetCellContents("A7"));
        }

        /// <summary>
        /// Check if SetCellContents replaces string content of existing cell 
        /// </summary>
        [TestMethod]
        public void SetCellContentsString4()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A7", new Formula());
            sheet.SetCellContents("A1", "a");
            sheet.SetCellContents("A7", "");
            Assert.AreEqual("", sheet.GetCellContents("A7"));
        }

        /// <summary>
        /// Check if SetCellContents puts a double in cell content
        /// </summary>
        [TestMethod]
        public void SetCellContentsDouble1()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 10);
            Assert.AreEqual(10.0, sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Check if SetCellContents replaces double content of existing cell 
        /// </summary>
        [TestMethod]
        public void SetCellContentsDouble2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A7", 5);
            sheet.SetCellContents("A1", 10);
            sheet.SetCellContents("A7", 3);
            Assert.AreEqual(3.0, sheet.GetCellContents("A7"));
        }

        /// <summary>
        /// Check if SetCellContents formula evaluates to correct answer
        /// </summary>
        [TestMethod]
        public void SetCellContentsFormula()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", new Formula());
            Formula f = (Formula) sheet.GetCellContents("A1");
            Assert.AreEqual(0.0, f.Evaluate(x => 0));
        }

        /// <summary>
        /// Check if SetCellContents formula evaluates to correct answer, stress test 1
        /// </summary>
        [TestMethod]
        public void SetCellContentsFormula1()
        {
            Spreadsheet sheet = new Spreadsheet();
            for (int i = 1; i <= 100; i++)
            {
                sheet.SetCellContents("x" + i, new Formula("A+" + i * 2));
                Formula temp = (Formula) (sheet.GetCellContents("x" + i));
                Assert.AreEqual(i * 2, temp.Evaluate( x => 0));
            }
            
        }

        /// <summary>
        /// Check if SetCellContents formula evaluates to correct answer, stress test 2
        /// </summary>
        [TestMethod]
        public void SetCellContentsFormula2()
        {
            Spreadsheet sheet = new Spreadsheet();
            for (int i = 1; i <= 1_000; i++)
            {
                sheet.SetCellContents("A" + i + i, new Formula("A+" + i * 2 * i + "/ 2"));
                Formula temp = (Formula)(sheet.GetCellContents("A" + i + i));
                Assert.AreEqual(3 + i * 2 * i / 2, temp.Evaluate(x => 3));
            }

        }
    }
}

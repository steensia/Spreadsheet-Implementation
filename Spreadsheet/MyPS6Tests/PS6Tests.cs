using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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
        public void GetCellValueTest()
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
        public void GetCellValueTest2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=x5");
            Assert.IsTrue(sheet.GetCellValue("A1").GetType() == typeof(FormulaError));
        }

        /// <summary>
        /// Check if GetCellValue evaluates a valid formula
        /// </summary>
        [TestMethod]
        public void GetCellValueTest3()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=10+4");
            Assert.AreEqual(14.0, sheet.GetCellValue("A1"));
        }
        /// <summary>
        /// Check if GetCellValue returns string
        /// </summary>
        [TestMethod]
        public void GetCellValueTest4()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "parameter");
            Assert.AreEqual("parameter", sheet.GetCellValue("A1"));
        }

        /// <summary>
        /// Check if GetCellValue throws InvalidNameException when name is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellValueTest5()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "parameter");
            sheet.GetCellContents(null);
        }

        /// <summary>
        /// Check if GetCellValue throws InvalidNameException when name is invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]

        public void GetCellValueTest6()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "parameter");
            sheet.GetCellContents("05");
        }

        /// <summary>
        /// Check if GetCellValue returns an empty value when cell does not exist
        /// </summary>
        [TestMethod]

        public void GetCellValueTest7()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "parameter");
            Assert.IsTrue("".Equals(sheet.GetCellContents("X2")));
        }

        /// <summary>
        /// Check to see if Save works and saves XML document.
        /// </summary>
        [TestMethod]
        public void SaveTest()
        {
            StringWriter s = new StringWriter();
            s.Write("../../spreadsheet1.xml");
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "ok");
            sheet.Save(s);
        }

        /// <summary>
        /// Check to see if SetContentsOfCell throws ArgumentNullExcepion
        /// when content is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetContentsOfCellTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", null);
        }

        /// <summary>
        /// Check to see if SetContentsOfCell throws InvalidNameException
        /// when name is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellTest2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "5");
        }

        /// <summary>
        /// Check to see if SetContentsOfCell throws InvalidNameException
        /// when name is invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellTest3()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("5A", "5");
        }

        /// <summary>
        /// Check if SetContentsOfCell throws FormulaFormatException
        /// when the remainder of formula cannot be parsed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SetContentsOfCellTest4()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=?");
        }

        /// <summary>
        /// Check if SetContentsOfCell throws CircularException
        /// when setting a cell causes circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsOfCellTest5()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=A2");
            sheet.SetContentsOfCell("A2", "=A1+5");
        }

        /// <summary>
        /// Check if SetContentsOfCell returns correct set of cells
        /// </summary>
        [TestMethod]
        public void SetContentsOfCellTest6()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=5");
            sheet.SetContentsOfCell("B1", "=A1*2");
            sheet.SetContentsOfCell("C1", "=B1+A1");
            Assert.IsTrue(sheet.SetContentsOfCell("A1", "5").SetEquals(new HashSet<string> { "A1", "B1", "C1" }));
        }

        /// <summary>
        /// Check if SetContentsOfCell replaces existing cell with formula
        /// </summary>
        [TestMethod]
        public void SetContentsOfCellTest7()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("X1", "2");
            sheet.SetContentsOfCell("Y1", "3");
            sheet.SetContentsOfCell("A1", "=5");
            sheet.SetContentsOfCell("A1", "=2+X1+Y1");
            Assert.AreEqual(7.0, sheet.GetCellValue("A1"));
        }

        /// <summary>
        /// Check if SetContentsOfCell sets cell empty after passing in empty string
        /// </summary>
        [TestMethod]
        public void SetContentsOfCellTest8()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A2", "");
            Assert.IsTrue("".Equals(sheet.GetCellValue("A2")));
        }

        /// <summary>
        /// Check if SetContentsOfCell overwrites a cell that had a string, with a string
        /// </summary>
        [TestMethod]
        public void SetContentsOfCellTest9()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A2", "bee");
            sheet.SetContentsOfCell("A2", "cat");
            Assert.IsTrue("cat".Equals(sheet.GetCellValue("A2")));
        }

        /// <summary>
        /// Check to see if third constructor throws error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void ThirdConstructorTest()
        {
            StringReader s = new StringReader("../../spreadsheet1.xml");
            Spreadsheet sheet = new Spreadsheet(s, new Regex(@"^.$"));
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
    }
}

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
            for (int i = 0; i < 1_000; i++)
            {
                s.SetContentsOfCell("A1" + i, "B2" + i);
                t.Add("A1" + i);
            }
            for (int i = 777; i < 1_000; i++)
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
        /// Check to see if second constructor accepts new C# regex
        /// </summary>
        [TestMethod]
        public void SecondConstructorTest()
        {
            Spreadsheet sheet = new Spreadsheet(new Regex(@"^[1-9]$"));
            sheet.SetContentsOfCell("7", "g");
            Assert.AreEqual("g", sheet.GetCellContents("7"));
        }

        /// <summary>
        /// Check to see if third constructor works with SampleSpreadsheet.xml
        /// </summary>
        [TestMethod]
        public void ThirdConstructorTest()
        {
            StreamReader s = new StreamReader("C:/Users/steen/source/repos/spreadsheet/Spreadsheet/Spreadsheet/SampleSavedSpreadsheet.xml");
            Spreadsheet sheet = new Spreadsheet(s, new Regex(@"^[a-zA-Z]+[1-9][0-9]*$"));
            Assert.AreEqual(1.5, sheet.GetCellContents("A1"));
            Assert.AreEqual("Hello", sheet.GetCellContents("B2"));
        }

        /// <summary>
        /// Check to see if third constructor throws IOException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ThirdConstructorTest2()
        {
            StreamReader s = new StreamReader("../../null.xml");
            Spreadsheet sheet = new Spreadsheet(s, new Regex(@"^.$"));
        }

        /// <summary>
        /// Check to see if third constructor throws SpreadsheetReadException
        /// when the contents of the source are inconsistent with schema
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void ThirdConstructorTest3()
        {
            StreamReader s = new StreamReader("C:/Users/steen/source/repos/spreadsheet/Spreadsheet/MyPS6Tests/spreadsheet2.xml");
            Spreadsheet sheet = new Spreadsheet(s, new Regex(@"^[a-zA-Z]+[1-9][0-9]*$"));
        }

        /// <summary>
        /// Check to see if third constructor throws SpreadsheetReadException
        /// when the IsValid string contains an invalid C# Regex
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void ThirdConstructorTest4()
        {
            StreamReader s = new StreamReader("C:/Users/steen/source/repos/spreadsheet/Spreadsheet/MyPS6Tests/spreadsheet3.xml");
            Spreadsheet sheet = new Spreadsheet(s, new Regex(@"^.$"));
        }

        /// <summary>
        /// Check to see if third constructor throws SpreadsheetReadException
        /// when there is a duplicate cell name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void ThirdConstructorTest5()
        {
            StreamReader s = new StreamReader("C:/Users/steen/source/repos/spreadsheet/Spreadsheet/MyPS6Tests/spreadsheet4.xml");
            Spreadsheet sheet = new Spreadsheet(s, new Regex(@"^[a-zA-Z]+[1-9][0-9]*$"));
        }

        /// <summary>
        /// Check to see if third constructor throws SpreadsheetReadException
        /// when there is an invalid cell name or formula (oldIsValid)
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void ThirdConstructorTest6()
        {
            StreamReader s = new StreamReader("C:/Users/steen/source/repos/spreadsheet/Spreadsheet/MyPS6Tests/spreadsheet1.xml");
            Spreadsheet sheet = new Spreadsheet(s, new Regex(@"^[a-zA-Z]+[1-9][0-9]*$"));
        }

        /// <summary>
        /// Check to see if third constructor throws SpreadsheetVersionException
        /// when there is an invalid cell name or formula (newIsValid)
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetVersionException))]
        public void ThirdConstructorTest7()
        {
            StreamReader s = new StreamReader("C:/Users/steen/source/repos/spreadsheet/Spreadsheet/MyPS6Tests/spreadsheet5.xml");
            Spreadsheet sheet = new Spreadsheet(s, new Regex(@"^[a-zA-Z]+[1-9][0-9]*$"));
        }

        /// <summary>
        /// Check to see if third constructor throws SpreadsheetReadException
        /// when there is a circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadException))]
        public void ThirdConstructorTest8()
        {
            StreamReader s = new StreamReader("C:/Users/steen/source/repos/spreadsheet/Spreadsheet/MyPS6Tests/spreadsheet6.xml");
            Spreadsheet sheet = new Spreadsheet(s, new Regex(@"^[a-zA-Z]+[1-9][0-9]*$"));
        }

        /// <summary>
        /// Check to see if Changed property returns false when no modification
        /// is made after creating a spreadsheet.
        /// </summary>
        [TestMethod]
        public void ChangedTest()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.IsTrue(sheet.Changed == false);
        }

        /// <summary>
        /// Check to see if Changed property returns false when no modification
        /// is made after creating a spreadsheet.
        /// </summary>
        [TestMethod]
        public void ChangedTest2()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("G7", "4");
            sheet.SetContentsOfCell("A1", "=5");
            sheet.SetContentsOfCell("B4", "=G7+A1");
            StreamWriter temp = new StreamWriter("../../sample.xml");
            sheet.Save(temp);
            Assert.IsTrue(sheet.Changed == false);
        }

        /// <summary>
        /// Check to see if Changed property returns false when no modification
        /// is made after creating a spreadsheet.
        /// </summary>
        [TestMethod]
        public void ChangedTest3()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("G7", "7");
            Assert.IsTrue(sheet.Changed == true);
        }

        ////Methods to create XML file
        ///// <summary>
        ///// Check to see if Save works and saves XML document.
        ///// </summary>
        //[TestMethod]
        //public void SaveTest()
        //{
        //    StreamWriter s = new StreamWriter("../../spreadsheet6.xml");
        //    Spreadsheet sheet = new Spreadsheet();
        //    sheet.SetContentsOfCell("A1", "=A2+5");
        //    sheet.SetContentsOfCell("B2", "=A1");
        //    sheet.Save(s);
        //}

        ///// <summary>
        ///// Create XML file with invalid regex
        ///// </summary>
        //[TestMethod]
        //public void SaveTest2()
        //{
        //    StreamWriter s = new StreamWriter("../../spreadsheet2.xml");
        //    Spreadsheet sheet = new Spreadsheet(new Regex(@"^[a-zA-Z]+[1-9][0-9]*$"));
        //    sheet.SetContentsOfCell("a2", "v");
        //    sheet.Save(s);
        //}

        ///// <summary>
        ///// Create XML file with invalid regex
        ///// </summary>
        //[TestMethod]
        //public void SaveTest3()
        //{
        //    StreamWriter s = new StreamWriter("../../spreadsheet3.xml");
        //    Spreadsheet sheet = new Spreadsheet(new Regex(@"^[a-zA-Z]+[1-9][0-9]*$"));
        //    sheet.SetContentsOfCell("x2", "a");
        //    sheet.Save(s);
        //}
    }
}

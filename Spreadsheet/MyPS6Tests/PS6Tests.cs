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
            sheet.GetCellContents(null);
        }

        /// <summary>
        /// Check if GetCellValue returns an empty value when cell does not exist
        /// </summary>
        [TestMethod]

        public void GetCellValueTest7()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "parameter");
            sheet.GetCellContents(null);
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
            sheet.SetContentsOfCell("A1", "=5");
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
        /// Check to see if third constructor throws error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void ThirdConstructorTest()
        {
            //StringWriter s = new StringWriter();
            //s.Write("../../spreadsheet1.xml");
            //Spreadsheet sheet = new Spreadsheet();
            //sheet.SetContentsOfCell("A1", "ok");
            //sheet.Save(s);
            StringReader s = new StringReader("sss");

            Spreadsheet sheet = new Spreadsheet(s, new Regex(@"^.$"));
        }
    }
}

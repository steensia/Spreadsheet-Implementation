﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dependencies;
using Formulas;

namespace SS
{
    /// <summary>
    /// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string s is a valid cell name if and only if it consists of one or more letters, 
    /// followed by a non-zero digit, followed by zero or more digits.
    /// 
    /// For example, "A15", "a15", "XY32", and "BC7" are valid cell names.  On the other hand, 
    /// "Z", "X07", and "hello" are not valid cell names.
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  
    /// In addition to a name, each cell has a contents and a value.  The distinction is
    /// important, and it is important that you understand the distinction and use
    /// the right term when writing code, writing comments, and asking questions.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In an empty spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError.
    /// The value of a Formula, of course, can depend on the values of variables.  The value 
    /// of a Formula variable is the value of the spreadsheet cell it names (if that cell's 
    /// value is a double) or is undefined (otherwise).  If a Formula depends on an undefined
    /// variable or on a division by zero, its value is a FormulaError.  Otherwise, its value
    /// is a double, as specified in Formula.Evaluate.
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// 
        /// </summary>
        private struct Cell
        {
            public string name;
            public object content;
            public object value;
            /// <summary>
            /// 
            /// </summary>
            public Cell(string name, object content, object value)
            {
                this.name = name;
                this.content = content;
                this.value = value;
            }
        }
        const String namePattern = @"^[a-zA-Z]+[1-9][0-9]*$";
        private Dictionary<string, Cell> cellMap;
        private DependencyGraph set;

        /// <summary>
        /// 
        /// </summary>
        public Spreadsheet()
        {
            this.cellMap = new Dictionary<string, Cell>();
            set = new DependencyGraph();
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> nonEmptyCells = new HashSet<string>();

            foreach (var name in cellMap.Keys)
            {
                nonEmptyCells.Add(name);
            }
            return new HashSet<string>(nonEmptyCells);
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if (name == null || !Regex.IsMatch(name, namePattern))
            {
                throw new InvalidNameException();
            }
            if (this.cellMap[name].content.Equals(null))
            {
                return "";
            }
            return this.cellMap[name].content;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, double number)
        {
            if (name == null || !Regex.IsMatch(name, namePattern))
            {
                throw new InvalidNameException();
            }
            if (!this.cellMap.ContainsKey(name))
            {
                set.AddDependency(name, number.ToString());
                this.cellMap.Add(name, new Cell(name, number, null));
            }
            else
            {
                set.AddDependency(name, number.ToString());
                this.cellMap[name] = new Cell(name, number, null);
            }
            return new HashSet<string>(GetCellsToRecalculate(name));           
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException();
            }
            if (name == null || !Regex.IsMatch(name, namePattern))
            {
                throw new InvalidNameException();
            }
            if (!this.cellMap.ContainsKey(name))
            {
                set.AddDependency(name, text);
                this.cellMap.Add(name, new Cell(name, text, null));
            }
            else
            {
                set.AddDependency(name, text);
                this.cellMap[name] = new Cell(name, text, null);
            }
            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// Requires that all of the variables in formula are valid cell names.
        /// 
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            HashSet<string> varSet = new HashSet<string>(formula.GetVariables());
            foreach (var variable in varSet)
            {
                if (!Regex.IsMatch(variable, namePattern))
                {
                    return null;
                }
            }
            if (name == null || !Regex.IsMatch(name, namePattern))
            {
                throw new InvalidNameException();
            }
            if (this.cellMap.ContainsKey(name))
            {
                set.RemoveDependency(name, formula.ToString());
                set.AddDependency(name, formula.ToString());

                this.cellMap[name] = new Cell(name, formula, null);
                
            }
            else
            {
                set.AddDependency(name, formula.ToString());
                this.cellMap.Add(name, new Cell(name, formula, null));
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            else if (!Regex.IsMatch(name, namePattern))
            {
                throw new InvalidNameException();
            }
            return set.GetDependents(name);
        }
    }
}

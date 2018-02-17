using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    /// spreadsheet consists of a regular expression (called IsValid below) and an infinite 
    /// number of named cells.
    /// 
    /// A string is a valid cell name if and only if (1) s consists of one or more letters, 
    /// followed by a non-zero digit, followed by zero or more digits AND (2) the C#
    /// expression IsValid.IsMatch(s.ToUpper()) is true.
    /// 
    /// For example, "A15", "a15", "XY32", and "BC7" are valid cell names, so long as they also
    /// are accepted by IsValid.  On the other hand, "Z", "X07", and "hello" are not valid cell 
    /// names, regardless of IsValid.
    /// 
    /// Any valid incoming cell name, whether passed as a parameter or embedded in a formula,
    /// must be normalized by converting all letters to upper case before it is used by this 
    /// this spreadsheet.  For example, the Formula "x3+a5" should be normalize to "X3+A5" before 
    /// use.  Similarly, all cell names and Formulas that are returned or written to a file must also
    /// be normalized.
    /// 
    /// A spreadsheet contains a unique cell corresponding to every possible cell name.  
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
        // Fields
        private Dictionary<string, Cell> cellMap;
        private DependencyGraph set;

        // Constant(s)
        const String cellNamePattern = @"^[a-zA-Z]+[1-9][0-9]*$";

        /// <summary>
        /// 
        /// </summary>
        public override bool Changed { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        /// <summary>
        /// Private struct Cell that contains a name, content, and value to represent in a spreadsheet
        /// </summary>
        private struct Cell
        {
            public string name;
            public object content;
            public object value;

            /// <summary>
            /// Cell constructor to initialize its name, content, and value
            /// </summary>
            public Cell(string name, object content, object value)
            {
                this.name = name;
                this.content = content;
                this.value = value;
            }

            /// <summary>
            /// New constructor to copy the contents of passed in Cell
            /// </summary>
            public Cell(Cell other)
            {
                this.name = other.name;
                this.content = other.content;
                this.value = other.value;
            }
        }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression accepts every string.
        /// A Spreadsheet contains a cell and each cell keeps tracks of its dependencies
        /// </summary>
        public Spreadsheet()
        {
            this.cellMap = new Dictionary<string, Cell>();
            this.set = new DependencyGraph();
        }

        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter
        public Spreadsheet(Regex isValid)
        {

        }

        /// Creates a Spreadsheet that is a duplicate of the spreadsheet saved in source.
        ///
        /// See the AbstractSpreadsheet.Save method and Spreadsheet.xsd for the file format 
        /// specification.  
        ///
        /// If there's a problem reading source, throws an IOException.
        ///
        /// Else if the contents of source are not consistent with the schema in Spreadsheet.xsd, 
        /// throws a SpreadsheetReadException.  
        ///
        /// Else if the IsValid string contained in source is not a valid C# regular expression, throws
        /// a SpreadsheetReadException.  (If the exception is not thrown, this regex is referred to
        /// below as oldIsValid.)
        ///
        /// Else if there is a duplicate cell name in the source, throws a SpreadsheetReadException.
        /// (Two cell names are duplicates if they are identical after being converted to upper case.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a 
        /// SpreadsheetReadException.  (Use oldIsValid in place of IsValid in the definition of 
        /// cell name validity.)
        ///
        /// Else if there is an invalid cell name or an invalid formula in the source, throws a
        /// SpreadsheetVersionException.  (Use newIsValid in place of IsValid in the definition of
        /// cell name validity.)
        ///
        /// Else if there's a formula that causes a circular dependency, throws a SpreadsheetReadException. 
        ///
        /// Else, create a Spreadsheet that is a duplicate of the one encoded in source except that
        /// the new Spreadsheet's IsValid regular expression should be newIsValid.
        public Spreadsheet(TextReader source, Regex newIsValid)
        {

        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> nonEmptyCells = new HashSet<string>();

            foreach (var name in cellMap)
            {
                // Add string if not empty, otherwise ignore
                if (!name.Value.content.Equals(""))
                {
                    nonEmptyCells.Add(name.Key);
                }
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
            if (name == null || !Regex.IsMatch(name, cellNamePattern))
            {
                throw new InvalidNameException();
            }
            // Return empty string if cell does not exist
            if (!this.cellMap.ContainsKey(name))
            {
                return "";
            }
            // Otherwise return the content of the cell
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
        protected override ISet<string> SetCellContents(string name, double number)
        {
            if (name == null || !Regex.IsMatch(name, cellNamePattern))
            {
                throw new InvalidNameException();
            }
            // Check if cell exists, then modify all links and set new content
            if (this.cellMap.ContainsKey(name))
            {
                set.ReplaceDependents(name, new HashSet<string>());
                this.cellMap[name] = new Cell(name, number, null);
            }
            // Otherwise, create the cell associated with a number
            else
            {
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
        protected override ISet<string> SetCellContents(string name, string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException();
            }
            if (name == null || !Regex.IsMatch(name, cellNamePattern))
            {
                throw new InvalidNameException();
            }
            // Check if empty string, then remove existing link of the cell and make its contents empty
            if (text.Equals(""))
            {
                set.ReplaceDependents(name, new HashSet<string>());
                this.cellMap[name] = new Cell(name, text, null);
            }
            // Check if cell exists, then modify all links and set new content
            else if (this.cellMap.ContainsKey(name))
            {
                set.ReplaceDependents(name, new HashSet<string>());
                this.cellMap[name] = new Cell(name, text, null);
            }
            // Otherwise, create the cell associated with a number
            else
            {
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
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            HashSet<string> varSet = new HashSet<string>(GetNamesOfAllNonemptyCells());
            if (name == null || !Regex.IsMatch(name, cellNamePattern))
            {
                throw new InvalidNameException();
            }
            // Check if cell already exists
            if (this.cellMap.ContainsKey(name))
            {
                // Preserve old cell and its links before modifying
                Cell oldCell = this.cellMap[name];
                HashSet<string> oldDentSet = new HashSet<string>(set.GetDependents(name));
                // Add links to each cell, replace old cell and check if circular dependency exists
                try
                {
                    foreach (var form in formula.GetVariables())
                    {
                        set.AddDependency(name, form);
                    }
                    this.cellMap[name] = new Cell(name, formula, null);
                    return new HashSet<string>(GetCellsToRecalculate(name));
                }
                // Revert to the previous cell and its links, then throw exception
                catch (CircularException)
                {
                    set.ReplaceDependents(name, oldDentSet);
                    this.cellMap[name] = new Cell(oldCell);
                    throw new CircularException();
                }
            }
            // Otherwise, replace old cell and create links between cells
            else
            {
                    foreach (var form in formula.GetVariables())
                    {
                        set.AddDependency(name, form);
                    }
                    this.cellMap[name] = new Cell(name, formula, null);
            }
            return new HashSet<string>(GetCellsToRecalculate(name));
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
            else if (!Regex.IsMatch(name, cellNamePattern))
            {
                throw new InvalidNameException();
            }
            // Return set of cell names that depend on a cell's name
            return new HashSet<string>(set.GetDependees(name));
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Save(TextWriter dest)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override object GetCellValue(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            throw new NotImplementedException();
        }
    }
}
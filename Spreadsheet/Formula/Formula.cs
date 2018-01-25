﻿// Skeleton written by Joe Zachary for CS 3500, January 2017

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        /// 

        private List<string> tokenList = new List<string>();

        bool lpFlag = false;
        bool rpFlag = false;
        bool opFlag = false;
        bool varFlag = false;
        bool doubleFlag = false;

        int lpCount = 0;
        int rpCount = 0;
        //int opCount = 0;
        //int varCount = 0;
        //int doubleCount = 0;

        const String lpPattern = @"\(";
        const String rpPattern = @"\)";
        const String opPattern = @"[\+\-*/]";
        const String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";
        const String doublePattern = @"?d*(?:\d*\.\d*)?$";

        public Formula(String formula)
        {
            var temp = GetTokens(formula);

            foreach(string token in temp)
            {
                tokenList.Add(token);
            }

            //Check if there is at least one token
            if(tokenList.Count < 1)
            {
                throw new FormulaFormatException("Must contain a valid token");
            }

            if (tokenList.Count == 1)
            {
                if (Regex.IsMatch(tokenList[0], doublePattern) || Regex.IsMatch(tokenList[0], varPattern))
                {
                    Formula form = new Formula(formula);
                }
                else
                {
                    throw new FormulaFormatException("This is an invalid token");
                }
            }
            else if(tokenList.Count == 2)
            {


            }
            //Check the first token if its valid
            if (Regex.IsMatch(tokenList[0], doublePattern) || Regex.IsMatch(tokenList[0], varPattern) || Regex.IsMatch(tokenList[0], lpPattern))
            {
            }
            else
            {
                throw new FormulaFormatException("This is an invalid token");
            }

            for(int i = 1; i < tokenList.Count -1; i++)
            {
                //Ensures that the closing parentheses should not exceed the number of opening parentheses
                if (lpCount != 0 && rpCount > lpCount)
                {
                    throw new FormulaFormatException("This is an invalid token");
                }
                //A token following an open parenthesis must be a number, variable, or open parenthesis
                if (lpFlag)
                {
                    if (Regex.IsMatch(tokenList[i], doublePattern) || Regex.IsMatch(tokenList[i], varPattern) || Regex.IsMatch(tokenList[i], lpPattern))
                    {
                        lpFlag = false;
                    }
                    else
                    {
                        throw new FormulaFormatException("This is an invalid token");
                    }
                }
                //A token following an operator must be a number, variable, or open parenthesis
                if (opFlag)
                {
                    if (Regex.IsMatch(tokenList[i], doublePattern) || Regex.IsMatch(tokenList[i], varPattern) || Regex.IsMatch(tokenList[i], lpPattern))
                    {
                        opFlag = false;
                    }
                    else
                    {
                        throw new FormulaFormatException("This is an invalid token");
                    }
                }
                //A token following a number must be an operator or closing parenthesis
                if (doubleFlag)
                {
                    if (Regex.IsMatch(tokenList[i], opPattern) || Regex.IsMatch(tokenList[i], rpPattern))
                    {
                        doubleFlag = false;
                    }
                    else
                    {
                        throw new FormulaFormatException("This is an invalid token");
                    }
                }
                //A token following a variable must be an operator or closing parenthesis
                if (varFlag)
                {
                    if (Regex.IsMatch(tokenList[i], opPattern) || Regex.IsMatch(tokenList[i], rpPattern))
                    {
                        varFlag = false;
                    }
                    else
                    {
                        throw new FormulaFormatException("This is an invalid token");
                    }
                }
                //A token following a closing parenthesis must be an operator or closing parenthesis
                if (rpFlag)
                {
                    if (Regex.IsMatch(tokenList[i], opPattern) || Regex.IsMatch(tokenList[i], rpPattern))
                    {
                        rpFlag = false;
                    }
                    else
                    {
                        throw new FormulaFormatException("This is an invalid token");
                    }
                }

                //Checks for opening parentheses
                if (Regex.IsMatch(tokenList[i], lpPattern))
                {
                    lpCount++;
                    lpFlag = true;
                }
                //Checks for closing parentheses
                else if (Regex.IsMatch(tokenList[i], rpPattern))
                {
                    rpCount++;
                    rpFlag = true;
                }
                //Checks for math operators
                else if (Regex.IsMatch(tokenList[i], opPattern))
                {
                    opFlag = true;
                }
                //Checks for variable token
                else if (Regex.IsMatch(tokenList[i], varPattern))
                {
                    varFlag = true;
                }
                //Checks for double literals
                else if (Double.TryParse(tokenList[i], out double result))
                {
                    doubleFlag = true;
                }
                else
                {
                    throw new FormulaFormatException("This is an invalid token");
                }
            }
            //The last token must be a number, variable, or closing parenthesis
            if(Regex.IsMatch(tokenList[tokenList.Count -1], doublePattern) || Regex.IsMatch(tokenList[tokenList.Count - 1], varPattern) || Regex.IsMatch(tokenList[tokenList.Count - 1], rpPattern))
            {
            }
            else
            {
                throw new FormulaFormatException("This is an invalid token");
            }
            //Either no parentheses or equal number of parentheses in the formula   
            if (lpCount == rpCount)
            {
                Formula form = new Formula(formula);
            }
            else
            {
                throw new FormulaFormatException("Must contain at least one token");
            }
        }
        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            return 0;
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens.
            // NOTE:  These patterns are designed to be used to create a pattern to split a string into tokens.
            // For example, the opPattern will match any string that contains an operator symbol, such as
            // "abc+def".  If you want to use one of these patterns to match an entire string (e.g., make it so
            // the opPattern will match "+" but not "abc+def", you need to add ^ to the beginning of the pattern
            // and $ to the end (e.g., opPattern would need to be @"^[\+\-*/]$".)
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";

            // PLEASE NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter that says
            // embedded white space should be ignored.  See below for an example of this.
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern.  It contains embedded white space that must be ignored when
            // it is used.  See below for an example of this.  This pattern is useful for 
            // splitting a string into tokens.
            String splittingPattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            // PLEASE NOTE:  Notice the second parameter to Split, which says to ignore embedded white space
            /// in the pattern.
            foreach (String s in Regex.Split(formula, splittingPattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string var);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    [Serializable]
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    [Serializable]
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    [Serializable]
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}

// Skeleton written by Joe Zachary for CS 3500, January 2017
// Steen Sia, 1/25/18

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
    public struct Formula
    {
        // Fields

        private string formula;

        // Constants 
        const String lpPattern = @"\(";
        const String rpPattern = @"\)";
        const String opPattern = @"[\+\-*/]";
        const String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";

        public Formula(String f, Normalizer N, Validator V)
        {
            List<string> tokenList = new List<string>(GetTokens(f));

            // Boolean variables to go into respective conditions
            bool lpFlag = false;
            bool rpFlag = false;
            bool opFlag = false;
            bool varFlag = false;
            bool doubleFlag = false;

            // Open and closing parenthesis counters to keep track of equality
            int lpCount = 0;
            int rpCount = 0;

            // Keep track of number of tokens
            int tokenCount = 0;

            // Check if there is at least one token
            if (tokenList.Count < 1)
            {
                throw new FormulaFormatException("Must contain a valid token");
            }
            else
            {
                // Condition for more than two tokens
                foreach (string token in tokenList)
                {
                    // Check if variable is legal
                    if (Regex.IsMatch(token, varPattern))
                    {
                        if(Regex.IsMatch(N(token), varPattern))
                        {
                            if(V(N(token)) == false)
                            {
                                throw new FormulaFormatException("This is not a legal variable");
                            }

                        }
                        else
                        {
                            throw new FormulaFormatException("This is not a legal variable");
                        }
                    }

                    // Check if the the number closing parentheses exceed the number of opening parentheses so far
                    if (lpCount < rpCount && rpCount != 0)
                    {
                        throw new FormulaFormatException("The number of closing parentheses should not exceed the number opening parentheses.");
                    }
                    // The first token must be a number, variable, or opening parenthesis
                    if (token == tokenList[0] && tokenCount < 1)
                    {
                        if (!(Double.TryParse(token, out double numTemp2) || Regex.IsMatch(N(token), varPattern) || Regex.IsMatch(token, lpPattern)))
                        {
                            throw new FormulaFormatException("First token must be a number, variable, or opening parenthesis");
                        }
                        else if (Double.TryParse(token, out double numTemp3))
                        {
                            doubleFlag = true;
                        }
                        else if (Regex.IsMatch(N(token), varPattern))
                        {
                            varFlag = true;
                        }
                        else if (Regex.IsMatch(token, lpPattern))
                        {
                            lpFlag = true;
                            lpCount++;
                        }
                    }
                    // Iterate through the rest of the tokens and see if they are valid
                    else
                    {
                        // A token following an opening parenthesis must be a number, variable, or open parenthesis
                        if (lpFlag)
                        {
                            if (Double.TryParse(token, out double numTemp2))
                            {
                                lpFlag = false;
                                doubleFlag = true;
                            }
                            else if (Regex.IsMatch(N(token), varPattern))
                            {
                                lpFlag = false;
                                doubleFlag = true;
                            }
                            else if (Regex.IsMatch(token, lpPattern))
                            {
                                lpFlag = true;
                                lpCount++;
                            }
                            else
                            {
                                throw new FormulaFormatException("This is an invalid token");
                            }
                        }
                        // A token following an operator must be a number, variable, or open parenthesis
                        else if (opFlag)
                        {
                            if (Double.TryParse(token, out double numTemp2))
                            {
                                opFlag = false;
                                doubleFlag = true;
                            }
                            else if (Regex.IsMatch(N(token), varPattern))
                            {
                                opFlag = false;
                                varFlag = true;
                            }
                            else if (Regex.IsMatch(token, lpPattern))
                            {
                                opFlag = false;
                                lpFlag = true;
                                lpCount++;
                            }
                            else
                            {
                                throw new FormulaFormatException("This is an invalid token");
                            }
                        }
                        // A token following a number must be an operator or closing parenthesis
                        else if (doubleFlag)
                        {
                            if (Regex.IsMatch(token, opPattern))
                            {
                                doubleFlag = false;
                                opFlag = true;
                            }
                            else if (Regex.IsMatch(token, rpPattern))
                            {
                                doubleFlag = false;
                                rpFlag = true;
                                rpCount++;
                            }
                            else
                            {
                                throw new FormulaFormatException("This is an invalid token");
                            }
                        }
                        // A token following a variable must be an operator or closing parenthesis
                        else if (varFlag)
                        {
                            if (Regex.IsMatch(token, opPattern))
                            {
                                varFlag = false;
                                opFlag = true;
                            }
                            else if (Regex.IsMatch(token, rpPattern))
                            {
                                varFlag = false;
                                rpFlag = true;
                                rpCount++;
                            }
                            else
                            {
                                throw new FormulaFormatException("This is an invalid token");
                            }
                        }
                        // A token following a closing parenthesis must be an operator or closing parenthesis
                        else if (rpFlag)
                        {
                            if (Regex.IsMatch(token, opPattern))
                            {
                                rpFlag = false;
                                opFlag = true;
                            }
                            else if (Regex.IsMatch(token, rpPattern))
                            {
                                rpFlag = true;
                                rpCount++;
                            }
                            else
                            {
                                throw new FormulaFormatException("This is an invalid token");
                            }
                        }
                    }
                    tokenCount++;
                }
                // The last token must be a number, variable, or closing parenthesis
                if (!(Double.TryParse(tokenList[tokenList.Count - 1], out double numTemp) || Regex.IsMatch(N(tokenList[tokenList.Count - 1]), varPattern) || Regex.IsMatch(tokenList[tokenList.Count - 1], rpPattern)))
                {
                    throw new FormulaFormatException("First token must be a number, variable, or closing parenthesis");
                }
                // The number of closing and opening parentheses should match
                if (lpCount != rpCount)
                {
                    throw new FormulaFormatException("The number of opening parentheses must match the number of closing parentheses");
                }
            }
            this.formula = f;
        }

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
        public Formula(String formula)
        {
            List<string> tokenList = new List<string>(GetTokens(formula));

            // Boolean variables to go into respective conditions
            bool lpFlag = false;
            bool rpFlag = false;
            bool opFlag = false;
            bool varFlag = false;
            bool doubleFlag = false;

            // Open and closing parenthesis counters to keep track of equality
            int lpCount = 0;
            int rpCount = 0;

            // Keep track of number of tokens
            int tokenCount = 0;

            // Check if there is at least one token
            if (tokenList.Count < 1)
            {
                throw new FormulaFormatException("Must contain a valid token");
            }
            else
            {
                // Condition for more than two tokens
                foreach (string token in tokenList)
                {
                    // Check if the the number closing parentheses exceed the number of opening parentheses so far
                    if (lpCount < rpCount && rpCount != 0)
                    {
                        throw new FormulaFormatException("The number of closing parentheses should not exceed the number opening parentheses.");
                    }
                    // The first token must be a number, variable, or opening parenthesis
                    if (token == tokenList[0] && tokenCount < 1)
                    {
                        if (!(Double.TryParse(token, out double numTemp2) || Regex.IsMatch(token, varPattern) || Regex.IsMatch(token, lpPattern)))
                        {
                            throw new FormulaFormatException("First token must be a number, variable, or opening parenthesis");
                        }
                        else if (Double.TryParse(token, out double numTemp3))
                        {
                            doubleFlag = true;
                        }
                        else if (Regex.IsMatch(token, varPattern))
                        {
                            varFlag = true;
                        }
                        else if (Regex.IsMatch(token, lpPattern))
                        {
                            lpFlag = true;
                            lpCount++;
                        }
                    }
                    // Iterate through the rest of the tokens and see if they are valid
                    else
                    {
                        // A token following an opening parenthesis must be a number, variable, or open parenthesis
                        if (lpFlag)
                        {
                            if (Double.TryParse(token, out double numTemp2))
                            {
                                lpFlag = false;
                                doubleFlag = true;
                            }
                            else if (Regex.IsMatch(token, varPattern))
                            {
                                lpFlag = false;
                                doubleFlag = true;
                            }
                            else if (Regex.IsMatch(token, lpPattern))
                            {
                                lpFlag = true;
                                lpCount++;
                            }
                            else
                            {
                                throw new FormulaFormatException("This is an invalid token");
                            }
                        }
                        // A token following an operator must be a number, variable, or open parenthesis
                        else if (opFlag)
                        {
                            if (Double.TryParse(token, out double numTemp2))
                            {
                                opFlag = false;
                                doubleFlag = true;
                            }
                            else if (Regex.IsMatch(token, varPattern))
                            {
                                opFlag = false;
                                varFlag = true;
                            }
                            else if (Regex.IsMatch(token, lpPattern))
                            {
                                opFlag = false;
                                lpFlag = true;
                                lpCount++;
                            }
                            else
                            {
                                throw new FormulaFormatException("This is an invalid token");
                            }
                        }
                        // A token following a number must be an operator or closing parenthesis
                        else if (doubleFlag)
                        {
                            if (Regex.IsMatch(token, opPattern))
                            {
                                doubleFlag = false;
                                opFlag = true;
                            }
                            else if (Regex.IsMatch(token, rpPattern))
                            {
                                doubleFlag = false;
                                rpFlag = true;
                                rpCount++;
                            }
                            else
                            {
                                throw new FormulaFormatException("This is an invalid token");
                            }
                        }
                        // A token following a variable must be an operator or closing parenthesis
                        else if (varFlag)
                        {
                            if (Regex.IsMatch(token, opPattern))
                            {
                                varFlag = false;
                                opFlag = true;
                            }
                            else if (Regex.IsMatch(token, rpPattern))
                            {
                                varFlag = false;
                                rpFlag = true;
                                rpCount++;
                            }
                            else
                            {
                                throw new FormulaFormatException("This is an invalid token");
                            }
                        }
                        // A token following a closing parenthesis must be an operator or closing parenthesis
                        else if (rpFlag)
                        {
                            if (Regex.IsMatch(token, opPattern))
                            {
                                rpFlag = false;
                                opFlag = true;
                            }
                            else if (Regex.IsMatch(token, rpPattern))
                            {
                                rpFlag = true;
                                rpCount++;
                            }
                            else
                            {
                                throw new FormulaFormatException("This is an invalid token");
                            }
                        }
                    }
                    tokenCount++;
                }
                // The last token must be a number, variable, or closing parenthesis
                if (!(Double.TryParse(tokenList[tokenList.Count - 1], out double numTemp) || Regex.IsMatch(tokenList[tokenList.Count - 1], varPattern) || Regex.IsMatch(tokenList[tokenList.Count - 1], rpPattern)))
                {
                    throw new FormulaFormatException("First token must be a number, variable, or closing parenthesis");
                }
                // The number of closing and opening parentheses should match
                if (lpCount != rpCount)
                {
                    throw new FormulaFormatException("The number of opening parentheses must match the number of closing parentheses");
                }
            }
            this.formula = formula;
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
            List<string> tokenList = new List<string>(GetTokens(this.formula));

            // Create two stacks to store values and operators
            Stack<double> valStack = new Stack<double>();
            Stack<string> opStack = new Stack<string>();

            // Loop through to see if the expression can be evaluated
            foreach (string t in tokenList)
            {
                // If the token is a number or a variable, check if * or / is at the top of the operator stack, pop 
                // the value stack, pop the operator stack, and apply the popped operator to t and the popped number. 
                // Push the result onto the value stack. Otherwise, push t onto the value stack
                if (Double.TryParse(t, out double numTemp) || Regex.IsMatch(t, varPattern))
                {
                    double result;
                    // If it is a variable, see if it is a valid double
                    if (numTemp == 0 && Regex.IsMatch(t, varPattern))
                    {
                        try
                        {
                            numTemp = lookup(t);
                        }
                        catch
                        {
                            throw new FormulaEvaluationException("This cannot be evaluated");
                        }
                    }
                    if (opStack.Count != 0 && opStack.Peek().Equals("*"))
                    {
                        result = valStack.Pop() * numTemp;
                        opStack.Pop();
                        valStack.Push(result);
                    }
                    else if (opStack.Count != 0 && opStack.Peek().Equals("/"))
                    {
                        result = valStack.Pop() / numTemp;
                        opStack.Pop();

                        if (result == 0 || numTemp == 0)
                        {
                            throw new FormulaEvaluationException("A division by zero is not allowed");
                        }
                        valStack.Push(result);
                    }
                    else
                    {
                        valStack.Push(numTemp);
                    }
                }

                // If + is at the top of the operator stack, pop the value stack twice and the operator stack once.  Apply the popped operator 
                // to the popped numbers. Push the result onto the value stack. Push t onto the operator stack, regardless of first step
                else if (t.Equals("+"))
                {
                    if (opStack.Count != 0 && opStack.Peek().Equals("+"))
                    {
                        double result = valStack.Pop() + valStack.Pop();
                        opStack.Pop();

                        valStack.Push(result);
                    }
                    if (opStack.Count != 0 && opStack.Peek().Equals("-"))
                    {
                        double temp = valStack.Pop();
                        double result = valStack.Pop() - temp;
                        opStack.Pop();

                        valStack.Push(result);
                    }
                    opStack.Push(t);
                }
                // If - is at the top of the operator stack, pop the value stack twice and the operator stack once.  Apply the popped operator 
                // to the popped numbers. Push the result onto the value stack. Push t onto the operator stack, regardless of first step
                else if (t.Equals("-"))
                {
                    if (opStack.Count != 0 && opStack.Peek().Equals("-"))
                    {
                        double temp = valStack.Pop();
                        double result = valStack.Pop() - temp;
                        opStack.Pop();

                        valStack.Push(result);
                    }
                    if (opStack.Count != 0 && opStack.Peek().Equals("+"))
                    {
                        double result = valStack.Pop() + valStack.Pop();
                        opStack.Pop();

                        valStack.Push(result);
                    }
                    opStack.Push(t);
                }
                // Simply push multiply or divide operators to operator stack
                else if (t.Equals("*") || t.Equals("/"))
                {
                    opStack.Push(t);
                }
                // Push opening parenthesis to operator stack
                else if (Regex.IsMatch(t, lpPattern))
                {
                    opStack.Push(t);
                }

                // When token is a closing parenthesis check if + or - is at the top of the operator stack, pop the value stack twice 
                // and the operator stack once. Apply the popped operator to the popped numbers. Push the result onto the value stack.
                else if (Regex.IsMatch(t, rpPattern))
                {
                    if (opStack.Count != 0 && opStack.Peek().Equals("+"))
                    {
                        double result = valStack.Pop() + valStack.Pop();
                        opStack.Pop();
                        valStack.Push(result);
                    }
                    else if (opStack.Count != 0 && opStack.Peek().Equals("-"))
                    {
                        double temp = valStack.Pop();
                        double result = valStack.Pop() - temp;
                        opStack.Pop();
                        valStack.Push(result);
                    }

                    // The top of operator stack will be opening parenthesis, just pop out of stack
                    opStack.Pop();


                    // If * or / is at the top of the operator stack, pop the value stack twice and the operator stack once. 
                    // Apply the popped operator to the popped numbers. Push the result onto the value stack.
                    if (opStack.Count != 0 && opStack.Peek().Equals("*"))
                    {
                        double result2 = valStack.Pop() * valStack.Pop();
                        opStack.Pop();
                        valStack.Push(result2);
                    }
                    else if (opStack.Count != 0 && opStack.Peek().Equals("/"))
                    {
                        double result2 = valStack.Pop();
                        if (valStack.Peek() == 0)
                        {
                            throw new FormulaEvaluationException("A division by zero is not allowed");
                        }
                        else
                        {
                            result2 = result2 / valStack.Pop();
                        }

                        opStack.Pop();

                        if (result2 == 0)
                        {
                            throw new FormulaEvaluationException("A division by zero is not allowed");
                        }
                        valStack.Push(result2);
                    }
                }
            }
            // After the tokens have been process, if the operator stack is empty
            // Return the only number in the value stack
            if (opStack.Count < 1)
            {
                return valStack.Pop();
            }
            // If operator stack is not empty, the operator is a + or - and there are two values in
            // the value stack. Just pop all tokens and return the result as the value of the expression.
            else
            {
                if (opStack.Count != 0 && opStack.Peek().Equals("+"))
                {
                    return valStack.Pop() + valStack.Pop();
                }
                else if (opStack.Count != 0 && opStack.Peek().Equals("-"))
                {
                    double temp = valStack.Pop();
                    opStack.Pop();
                    return valStack.Pop() - temp;
                }
                else
                {
                    throw new FormulaEvaluationException("Cannot evaluate this expression");
                }
            }
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

        /// <summary>
        /// Returns an ISet<String> that contains each distinct variable (in normalized form)
        /// that appears in the Formula.
        /// </summary>
        private ISet<string> GetVariables(Normalizer N)
        {
            HashSet<string> Set = new HashSet<string>(GetTokens(formula));
            foreach (string token in Set)
            {
                if (Regex.IsMatch(N(token), varPattern))
                {

                }
            }
            return new HashSet<string>();
        }

        public override string ToString()
        {
            return base.ToString();
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
    /// The purpose of a Normalizer is to convert variables into a canonical form.
    /// </summary>
    public delegate string Normalizer(string s);

    /// <summary>
    /// The purpose of a Validator is to impose extra restrictions on the validity of a variable, 
    /// beyond the ones already built into the Formula definition.  
    /// </summary>
    public delegate bool Validator(string s);

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

using System;
using System.Collections.Generic;
using Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyPS4aTests
{
    [TestClass]
    public class PS4aTests
    {
        /// <summary>
        /// Check if toString method works when creating a new formula. Formula f1 
        /// and f2 should be identical after the statement below:
        /// Formula f2 = new Formula(f1.ToString(), s => s, s => true)
        /// </summary>
        [TestMethod]
        public void ToStringTest1()
        {
            Formula f1 = new Formula("x + y", s => s.ToUpper(), s => true);
            Formula f2 = new Formula(f1.ToString(), s => s, s => true);
            Assert.IsTrue(f1.ToString().Equals(f2.ToString()));
        }

        /// <summary>
        /// Check if toString method works when creating a new formula with 
        /// the default constructor.
        /// </summary>
        [TestMethod]
        public void ToStringTest2()
        {
            Formula f0 = new Formula();
            Assert.IsTrue("0".Equals(f0.ToString()));
        }

        /// <summary>
        /// Check if toString method represents the constructed formula
        /// </summary>
        [TestMethod]
        public void ToStringTest3()
        {
            Formula f0 = new Formula("a + b + c");
            // ignore white space
            Assert.IsTrue("a+b+c".Equals(f0.ToString()));
        }

        /// <summary>
        /// Check if toString method works correctly when evaluating
        /// another constructor identical to it.
        /// </summary>
        [TestMethod]
        public void ToStringTest4()
        {
            Formula f0 = new Formula("a + b + c", s => s.ToUpper(), s => true);
            Formula f1 = new Formula(f0.ToString());
            Assert.IsTrue(f0.Evaluate(s => 3).Equals(f1.Evaluate(s => 3)));
        }

        /// <summary>
        /// Check if zero argument constructor is equal to a formula that constructs "0"
        /// </summary>
        [TestMethod]
        public void ConstructorZeroArgument()
        {
            Formula f0 = new Formula();
            Formula f1 = new Formula("0", s => "s", s => true);
            Assert.IsTrue(f0.ToString().Equals(f1.ToString()));
        }

        /// <summary>
        /// Check evaluate works on constructor made by toString method.
        /// </summary>
        [TestMethod]
        public void EvaluateToStringFormula()
        {
            Formula f0 = new Formula("a * b", s => s.ToUpper(), s => true);
            Formula f1 = new Formula(f0.ToString());
            Assert.AreEqual(4, f1.Evaluate(s => 2));
        }

        /// <summary>
        /// Check evaluate works after it is normalized to upper case
        /// </summary>
        [TestMethod]
        public void EvaluateTest1()
        {
            Formula f0 = new Formula("x + 2", s => s.ToUpper(), s => true);
            Assert.AreEqual(7, f0.Evaluate(Lookup));
        }

        /// <summary>
        /// Check evaluate works after it is normalized to lower case
        /// </summary>
        [TestMethod]
        public void EvaluateTest2()
        {
            Formula f0 = new Formula("X + 3", s => s.ToLower(), s => true);
            Assert.AreEqual(7, f0.Evaluate(Lookup));
        }

        /// <summary>
        /// Check if GetVariables returns the normalized variable tokens
        /// </summary>
        [TestMethod]
        public void GetVariablesTest1()
        {
            Formula f1 = new Formula("x+y+z", s => s.ToUpper(), s => true);
            HashSet<string> set = new HashSet<string>(f1.GetVariables());
            HashSet<string> set2 = new HashSet<string>() { "X", "Y", "Z" };
            Assert.IsTrue(set2.SetEquals(set));
        }

        /// <summary>
        /// Check if GetVariables returns an empty list if contain no variables
        /// </summary>
        [TestMethod]
        public void GetVariablesTest2()
        {
            Formula f1 = new Formula("1+2", s => s.ToUpper(), s => true);
            HashSet<string> set = new HashSet<string>(f1.GetVariables());
            HashSet<string> set2 = new HashSet<string>() { };
            Assert.IsTrue(set2.SetEquals(set));
        }

        /// <summary>
        /// Check if GetVariables returns identical variables using new constructor
        /// </summary>
        [TestMethod]
        public void GetVariablesTest3()
        {
            Formula f1 = new Formula("b+a", s => s, s => true);
            HashSet<string> set = new HashSet<string>(f1.GetVariables());
            HashSet<string> set2 = new HashSet<string>() { "b", "a" };
            Assert.IsTrue(set2.SetEquals(set));
        }

        /// <summary>
        /// Check if GetVariables returns identical variables using old constructor
        /// </summary>
        [TestMethod]
        public void GetVariablesTest4()
        {
            Formula f1 = new Formula("z + x");
            HashSet<string> set = new HashSet<string>(f1.GetVariables());
            HashSet<string> set2 = new HashSet<string>() { "z", "x" };
            Assert.IsTrue(set2.SetEquals(set));
        }

        /// <summary>
        /// Check if a FormatFormulaException is thrown when
        /// validator is set to false.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ValidatorTest1()
        {
            Formula f0 = new Formula("A + B", s => s.ToLower(), s => false);
        }

        /// <summary>
        /// Check if ArgumentNullException is thrown when validator is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidatorNullTest1()
        {
            Formula f1 = new Formula("x+y", s => s.ToUpper(), null);
        }

        /// <summary>
        /// Check if a FormatFormulaException is thrown when
        /// normalizer changes variable to invalid token.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void NormalizerTest1()
        {
            Formula f0 = new Formula("A + B", s => "?", s => true);
        }

        /// <summary>
        /// Check if ArgumentNullException is thrown when normalizer is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NormalizerNullTest1()
        {
            Formula f1 = new Formula("x+y", s => null, s => true);
        }

        /// <summary>
        /// Check if Formula constructor throws an ArgumentNullException
        /// when null is passed in the first parameter
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest1()
        {
            Formula f0 = new Formula(null, s => s.ToLower(), s => true);
        }

        /// <summary>
        /// Check if the old Formula constructor is able to evaluate the token.
        /// </summary>
        [TestMethod]
        public void OldConstructorTest1()
        {
            Formula f0 = new Formula("7");
            Assert.AreEqual(7, f0.Evaluate(s => 7));
        }

        /// <summary>
        /// Check if Evaluate method outputs 0 when called on a default constructor
        /// </summary>
        [TestMethod]
        public void EvaluateDefaultConstructor()
        {
            Formula f0 = new Formula();
            Assert.AreEqual(f0.Evaluate(s => 2), 0);
        }

        /// <summary>
        /// Check if Evaluate throws an ArgumentNullException when null is passed in
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EvaluateNullCheck()
        {
            Formula f0 = new Formula();
            f0.Evaluate(null);
        }

        /// <summary>
        /// Check if Evaluate throws FormulaEvaluationError when 0 is at the top
        /// of value stack and about to divide a number with it.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void EvaluateDivideByZeroTest()
        {
            Formula f0 = new Formula("(4-2) / (3 - x)");
            Assert.AreEqual(f0.Evaluate(s => 3), 0);
        }

        /// <summary>
        /// A Lookup method that maps x to 4.0, X to 5.0
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "X": return 5.0;
                default: throw new UndefinedVariableException(v);
            }
        }
    }
}

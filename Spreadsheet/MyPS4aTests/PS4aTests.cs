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
            Formula f1 = new Formula("x+y", s => "s", s => true);
            Formula f2 = new Formula(f1.ToString(), s => s, s => true);
            Assert.IsTrue(f1.Equals(f2));
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
        /// Check if GetVariables returns the normalized variable tokens
        /// </summary>
        [TestMethod]
        public void GetVariablesTest1()
        {
            Formula f1 = new Formula("x+y", s => s.ToUpper(), s => true);
            HashSet<string> set = new HashSet<string>(f1.GetVariables());
            HashSet<string> set2 = new HashSet<string>() { "X", "Y" };
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
            f0.Evaluate(s => 3);
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
            f0.Evaluate(s => 3);
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


    }
}

using System;
using Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyPS4aTests
{
    [TestClass]
    public class PS4aTests
    {
        /// <summary>
        /// Check if toString method works when creatin a new formula. Formula f1 
        /// and f2 should be identical after the statement below:
        /// Formula f2 = new Formula(f1.ToString(), s => s, s => true)
        /// </summary>
        [TestMethod]
        public void ToStringTest1()
        {
            Formula f1 = new Formula("x+y", s => s.ToUpper(), s => true);
            Formula f2 = new Formula(f1.ToString(), s => s, s => true);
            Assert.IsTrue(f1.Equals(f2));
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDependencyGraph
{
    /// <summary>
    /// These are comprehensive tests to ensure that the specifications
    /// of a DependencyGraph is met.
    /// </summary>
    [TestClass]
    public class TestDependencyGraph
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void CheckNullHasDependents()
        {
            DependencyGraph ok = new DependencyGraph();
        }
    }
}

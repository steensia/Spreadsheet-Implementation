using System;
using Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyPS4bTests
{
    [TestClass]
    public class PS4bTests
    {
        /// <summary>
        /// Check if the new constructor produces the same set of dependencies
        /// </summary>
        [TestMethod]
        public void TestConstructor1()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            DependencyGraph graph2 = new DependencyGraph(graph);

            Assert.IsTrue(graph.Equals(graph2));
        }

        /// <summary>
        /// Check if the new DependencyGraph is independent of the first one
        /// by removing a dependency.
        /// </summary>
        [TestMethod]
        public void TestConstructor2()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            DependencyGraph graph2 = new DependencyGraph(graph);

            graph2.RemoveDependency("a", "b");
            Assert.AreEqual(0, graph2.Size);
        }
    }
}

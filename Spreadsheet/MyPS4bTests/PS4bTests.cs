using System;
using System.Collections.Generic;
using Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyPS4bTests
{
    [TestClass]
    public class PS4bTests
    {
        /// <summary>
        /// Check if the new constructor produces the same set of dependencies using GetDependents
        /// </summary>
        [TestMethod]
        public void TestConstructor1()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            graph.AddDependency("a", "c");
            graph.AddDependency("a", "d");
            graph.AddDependency("a", "e");
            DependencyGraph graph2 = new DependencyGraph(graph);

            HashSet<string> set = new HashSet<string>(graph.GetDependents("a"));
            HashSet<string> set2 = new HashSet<string>(graph2.GetDependents("a"));
            Assert.IsTrue(set.SetEquals(set2));
        }

        /// <summary>
        /// Check if the new constructor produces the same set of dependencies using GetDependees
        /// </summary>
        [TestMethod]
        public void TestConstructor2()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("w", "s");
            graph.AddDependency("x", "s");
            graph.AddDependency("y", "s");
            graph.AddDependency("z", "s");
            DependencyGraph graph2 = new DependencyGraph(graph);

            HashSet<string> set = new HashSet<string>(graph.GetDependees("s"));
            HashSet<string> set2 = new HashSet<string>(graph2.GetDependees("s"));
            Assert.IsTrue(set.SetEquals(set2));
        }

        /// <summary>
        /// Check if the new DependencyGraph is independent of the first one
        /// by removing a dependency.
        /// </summary>
        [TestMethod]
        public void TestConstructor3()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            DependencyGraph graph2 = new DependencyGraph(graph);

            graph2.RemoveDependency("a", "b");
            Assert.AreEqual(0, graph2.Size);
        }

        /// <summary>
        /// Check if the new DependencyGraph throws a null argument exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor4()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("a", "b");
            DependencyGraph graph2 = new DependencyGraph(null);
        }
    }
}

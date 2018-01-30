using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;
using System.Collections.Generic;

namespace TestDependencyGraph
{
    /// <summary>
    /// These are comprehensive tests to ensure that the specifications
    /// of a DependencyGraph is met.
    /// </summary>
    [TestClass]
    public class DependencyGraphTestCases
    {
        // Size test

        /// <summary>
        /// Check if size method returns 0 in an empty dependency graph
        /// </summary>
        [TestMethod]
        public void SizeWithNoDependency()
        {
            DependencyGraph graph = new DependencyGraph();
            Assert.AreEqual(0, graph.Size, 1e-6);
        }

        /// <summary>
        /// Check if size method returns 0 in an empty dependency graph
        /// </summary>
        [TestMethod]
        public void SizeWithDependencies()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("s", "t");
            Assert.AreEqual(1, graph.Size, 1e-6);
        }

        // HasDependents Tests

        /// <summary>
        /// Check if HasDependents throws a null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HasDependentsCheckNull()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.HasDependents(null);
        }

        /// <summary>
        /// Check if dependent is in the DependencyGraph
        /// </summary>
        [TestMethod]
        public void HasDependent_Yes()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("s", "t");
            Assert.AreEqual(true, graph.HasDependents("s"));
        }

        /// <summary>
        /// Check if dependent is not in the DependencyGraph
        /// </summary>
        [TestMethod]
        public void HasDependent_No()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("u", "v");
            Assert.AreEqual(false, graph.HasDependents("y"));
        }

        // HasDependees Tests

        /// <summary>
        /// Check if HasDependees throws a null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HasDependeesCheckNull()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.HasDependees(null);
        }


        /// <summary>
        /// Check if dependees is in the DependencyGraph
        /// </summary>
        [TestMethod]
        public void HasDependees_Yes()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("s", "t");
            Assert.AreEqual(true, graph.HasDependees("t"));
        }

        /// <summary>
        /// Check if dependees is not in the DependencyGraph
        /// </summary>
        [TestMethod]
        public void HasDependees_No()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("u", "v");
            Assert.AreEqual(false, graph.HasDependees("x"));
        }

        // GetDependents Tests

        /// <summary>
        /// Check if GetDependents throws a null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDependentsCheckNull()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.GetDependents(null);
        }

        /// <summary>
        /// Check if GetDependents returns dependents
        /// </summary>
        [TestMethod]
        public void GetDependentsTwoDependents()
        {
            DependencyGraph graph = new DependencyGraph();
            List<String> test = new List<string>();
            graph.AddDependency("s", "1");
            graph.AddDependency("s", "2");
            foreach(string n in graph.GetDependents("s"))
            {
                test.Add(n);
            }
            Assert.AreEqual("s", test[0]);
            Assert.AreEqual("s", test[1]);
        }

        // GetDependees Tests

        /// <summary>
        /// Check if GetDependees throws a null exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDependeesCheckNull()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.GetDependees(null);
        }

        // AddDependency Tests

        /// <summary>
        /// Check if AddDependency throws a null exception to first argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddDependencyCheckNull1()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(null, "DG");
        }

        /// <summary>
        /// Check if AddDependency throws a null exception to second argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddDependencyCheckNull2()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("DG", null);
        }

        /// <summary>
        /// Check if AddDependency throws a null exception to both arguments
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddDependencyCheckNull3()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency(null, null);
        }

        /// <summary>
        /// Check if AddDependency deals with a duplicate dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddDependencyDuplicate()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.AddDependency("s", "t");
            graph.AddDependency("s", "t");
        }

        // RemoveDependency Tests

        /// <summary>
        /// Check if RemoveDependency throws a null exception to first argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveDependencyCheckNull1()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.RemoveDependency(null, "DG");
        }

        /// <summary>
        /// Check if RemoveDependency throws a null exception to second argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveDependencyCheckNull2()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.RemoveDependency("DG", null);
        }

        /// <summary>
        /// Check if RemoveDependency throws a null exception to both arguments
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveDependencyCheckNull3()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.RemoveDependency(null, null);
        }

        /// <summary>
        /// Check if RemoveDependency deals with non-existent dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveDependencyInvalidDependency()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.RemoveDependency("s", "t");
        }

        // RemoveDependents Tests

        /// <summary>
        /// Check if RemoveDependents deals throw null exception to first argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveDependentsNull1()
        {
            DependencyGraph graph = new DependencyGraph();
            List<string> newDependents = new List<string>();
            graph.ReplaceDependents(null, newDependents);
        }

        /// <summary>
        /// Check if RemoveDependents deals throw null exception to second argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveDependentsNull2()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.ReplaceDependents("s", null);
        }

        /// <summary>
        /// Check if RemoveDependents deals throw null exception to both arguments
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveDependentsNull3()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.ReplaceDependents(null, null);
        }

        // RemoveDependees Tests

        /// <summary>
        /// Check if RemoveDependees deals throw null exception to first argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveDependeesNull1()
        {
            DependencyGraph graph = new DependencyGraph();
            List<string> newDependees = new List<string>();
            graph.ReplaceDependees(null, newDependees);
        }

        /// <summary>
        /// Check if RemoveDependees deals throw null exception to second argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveDependeesNull2()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.ReplaceDependees("s", null);
        }

        /// <summary>
        /// Check if RemoveDependees deals throw null exception to both arguments
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveDependeesNull3()
        {
            DependencyGraph graph = new DependencyGraph();
            graph.ReplaceDependees(null, null);
        }
    }
}

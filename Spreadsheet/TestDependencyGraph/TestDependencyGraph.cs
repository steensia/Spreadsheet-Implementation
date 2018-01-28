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
    public class TestDependencyGraph
    {
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

using System;
using System.Collections.Generic;
using System.Linq;
using Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyGraphTestCases
{
    [TestClass]
    public class DependencyGraphTest
    {
        /// <summary>
        /// These are comprehensive tests to ensure that the specifications
        /// of a DependencyGraph is met.
        /// </summary>
        [TestClass]
        public class DependencyGraphTestCases
        {
            // Size tests

            /// <summary>
            /// Check if size method returns 0 when there is no DependencyGraph
            /// </summary>
            [TestMethod]
            public void SizeWithNoDependency()
            {
                DependencyGraph graph = new DependencyGraph();
                Assert.AreEqual(0, graph.Size);
            }

            /// <summary>
            /// Check if size method returns 1 with one DependencyGraph
            /// </summary>
            [TestMethod]
            public void SizeWithOneDependency()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.AddDependency("s", "t");
                Assert.AreEqual(1, graph.Size);
            }

            /// <summary>
            /// Check if size method returns correct size for 100_000 DependencyGraphs
            /// </summary>
            [TestMethod]
            public void SizeWithOneHundredThousandDependencies()
            {
                DependencyGraph graph = new DependencyGraph();
                for (int i = 0; i < 100_000; i++)
                {
                    graph.AddDependency(i + "", i + "" + i);
                }
                Assert.AreEqual(100_000, graph.Size);
            }

            // HasDependents Tests

            /// <summary>
            /// Check if HasDependents throws an argument null exception
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void HasDependentsNullCheck()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.HasDependents(null);
            }

            /// <summary>
            /// Check if return true when put in a valid dependent
            /// </summary>
            [TestMethod]
            public void HasDependentValidDependent()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.AddDependency("s", "t");
                Assert.AreEqual(true, graph.HasDependents("s"));
            }

            /// <summary>
            /// Check if returns false when putting in non-existent dependent
            /// </summary>
            [TestMethod]
            public void HasDependentInvalidDependent()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.AddDependency("u", "v");
                Assert.AreEqual(false, graph.HasDependents("y"));
            }

            // HasDependees Tests

            /// <summary>
            /// Check if HasDependees throws an argument null exception
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void HasDependeesNullCheck()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.HasDependees(null);
            }

            /// <summary>
            /// Check if return true when putting in a valid dependee
            /// </summary>
            [TestMethod]
            public void HasDependeesValidDependee()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.AddDependency("s", "t");
                Assert.AreEqual(true, graph.HasDependees("t"));
            }

            /// <summary>
            /// Check if return false when putting in non-existent dependee
            /// </summary>
            [TestMethod]
            public void HasDependeesInvalidDependee()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.AddDependency("u", "v");
                Assert.AreEqual(false, graph.HasDependees("x"));
            }

            // GetDependents Tests

            /// <summary>
            /// Check if GetDependents throws a null exception with null input
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void GetDependentsNullCheck()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.GetDependents(null);
            }

            /// <summary>
            /// Check if GetDependents returns an empty list when asking for dependents from a dependent
            /// </summary>
            [TestMethod]
            public void GetDependentsReturnAnEmptyList()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.AddDependency("a", "b");
                HashSet<string> temp = new HashSet<string>(graph.GetDependents("b").ToList());
                Assert.AreEqual(0, temp.Count);
            }

            /// <summary>
            /// Check if GetDependents matches the correct dependents
            /// </summary>
            [TestMethod]
            public void GetDependentsTwoValidParents()
            {
                DependencyGraph graph = new DependencyGraph();
                List<string> childList = new List<string>();
                graph.AddDependency("s", "1");
                graph.AddDependency("s", "2");
                childList = graph.GetDependents("s").ToList();
                Assert.AreEqual("1", childList[0]);
                Assert.AreEqual("2", childList[1]);
            }

            /// <summary>
            /// Check if GetDependents matches all 100_000 dependents
            /// </summary>
            [TestMethod]
            public void GetDependentsStressTest1()
            {
                DependencyGraph graph = new DependencyGraph();

                for (int i = 0; i < 100_000; i++)
                {
                    graph.AddDependency("s", "" + i);
                }
                HashSet<string> parentList = new HashSet<string>(graph.GetDependents("s").ToList());
                for (int i = 0; i < 100_000; i++)
                {
                    Assert.AreEqual(true, parentList.Contains("" + i));
                }
            }

            /// <summary>
            /// Check if GetDependents matches all 1_000_000 dependents
            /// </summary>
            [TestMethod]
            public void GetDependentsStressTest2()
            {
                DependencyGraph graph = new DependencyGraph();

                for (int i = 0; i < 1_000_000; i++)
                {
                    graph.AddDependency("s", "" + i);
                }
                HashSet<string> parentList = new HashSet<string>(graph.GetDependents("s").ToList());
                for (int i = 0; i < 1_000_000; i++)
                {
                    Assert.AreEqual(true, parentList.Contains("" + i));
                }
            }

            /// <summary>
            /// Check if GetDependents returns dependents with 50_000 for each of the two dependees
            /// </summary>
            [TestMethod]
            public void GetDependentsStressTest3()
            {
                DependencyGraph graph = new DependencyGraph();

                for (int i = 0; i < 50_000; i++)
                {
                    graph.AddDependency("s", "" + i);
                }
                for (int i = 50_000; i < 100_000; i++)
                {
                    graph.AddDependency("t", "" + i + "s");
                }

                HashSet<string> parentList = new HashSet<string>(graph.GetDependents("s").ToList());
                HashSet<string> parentList2 = new HashSet<string>(graph.GetDependents("t").ToList());
                for (int i = 0; i < 50_000; i++)
                {
                    Assert.AreEqual(true, parentList.Contains("" + i));
                }
                for (int i = 50_000; i < 100_000; i++)
                {
                    Assert.AreEqual(true, parentList2.Contains("" + i + "s"));
                }
            }

            // GetDependees Tests

            /// <summary>
            /// Check if GetDependees throws a null exception
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void GetDependeesNullCheck()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.GetDependees(null);
            }

            /// <summary>
            /// Check if GetDependees returns an empty list when asking for dependents from a dependent
            /// </summary>
            [TestMethod]
            public void GetDependeesReturnAnEmptyList()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.AddDependency("a", "b");
                HashSet<string> temp = new HashSet<string>(graph.GetDependees("a").ToList());
                Assert.AreEqual(0, temp.Count);
            }

            /// <summary>
            /// Check if GetDependees returns dependents
            /// </summary>
            [TestMethod]
            public void GetDependeesTwoValidChildren()
            {
                DependencyGraph graph = new DependencyGraph();
                List<string> childList = new List<string>();
                graph.AddDependency("s", "1");
                graph.AddDependency("t", "1");
                childList = graph.GetDependees("1").ToList();
                Assert.AreEqual("s", childList[0]);
                Assert.AreEqual("t", childList[1]);
            }

            /// <summary>
            /// Check if GetDependees matches all 100_000 dependees
            /// </summary>
            [TestMethod]
            public void GetDependeesStressTest1()
            {
                DependencyGraph graph = new DependencyGraph();

                for (int i = 0; i < 100_000; i++)
                {
                    graph.AddDependency("" + i, "t");
                }
                HashSet<string> parentList = new HashSet<string>(graph.GetDependees("t").ToList());
                for (int i = 0; i < 100_000; i++)
                {
                    Assert.AreEqual(true, parentList.Contains("" + i));
                }
            }

            /// <summary>
            /// Check if GetDependees matches all 1_000_000 dependees
            /// </summary>
            [TestMethod]
            public void GetDependeesStressTest2()
            {
                DependencyGraph graph = new DependencyGraph();

                for (int i = 0; i < 1_000_000; i++)
                {
                    graph.AddDependency("" + i, "t");
                }
                HashSet<string> parentList = new HashSet<string>(graph.GetDependees("t").ToList());
                for (int i = 0; i < 1_000_000; i++)
                {
                    Assert.AreEqual(true, parentList.Contains("" + i));
                }
            }

            /// <summary>
            /// Check if GetDependees returns dependents with 50_000 for each of the two dependees
            /// </summary>
            [TestMethod]
            public void GetDependeesStressTest3()
            {
                DependencyGraph graph = new DependencyGraph();

                for (int i = 0; i < 50_000; i++)
                {
                    graph.AddDependency("" + i, "t");
                }
                for (int i = 50_000; i < 100_000; i++)
                {
                    graph.AddDependency("" + i + "s", "r");
                }

                HashSet<string> parentList = new HashSet<string>(graph.GetDependees("t").ToList());
                HashSet<string> parentList2 = new HashSet<string>(graph.GetDependees("r").ToList());
                for (int i = 0; i < 50_000; i++)
                {
                    Assert.AreEqual(true, parentList.Contains("" + i));
                }
                for (int i = 50_000; i < 100_000; i++)
                {
                    Assert.AreEqual(true, parentList2.Contains("" + i + "s"));
                }
            }

            // AddDependency Tests

            /// <summary>
            /// Check if AddDependency throws a null exception to first argument
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void AddDependencyNullCheck()
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
            public void AddDependencyDuplicate()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.AddDependency("s", "t");
                graph.AddDependency("s", "t");
                graph.AddDependency("v", "v");
                Assert.AreEqual(2, graph.Size);
            }

            /// <summary>
            /// Check if AddDependency deals with dependency using same parent
            /// </summary>
            [TestMethod]
            public void AddDependencySameParent()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.AddDependency("s", "t");
                graph.AddDependency("s", "p");
                Assert.AreEqual(2, graph.Size);
            }

            /// <summary>
            /// Check if AddDependency deals with dependency using same child
            /// </summary>
            [TestMethod]
            public void AddDependencySameChild()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.AddDependency("s", "t");
                graph.AddDependency("p", "t");
                Assert.AreEqual(2, graph.Size);
            }

            /// <summary>
            /// Check if AddDependency deals with dependencies using same parent and child
            /// </summary>
            [TestMethod]
            public void AddDependencySameParentAndChild()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.AddDependency("s", "t");
                graph.AddDependency("p", "t");
                graph.AddDependency("r", "o");
                graph.AddDependency("r", "w");
                Assert.AreEqual(4, graph.Size);
            }

            /// <summary>
            /// Check if AddDependency deals with 100,000 dependencies using same parent
            /// </summary>
            [TestMethod]
            public void AddDependencyStressTest1()
            {
                DependencyGraph graph = new DependencyGraph();
                for (int i = 0; i < 100_000; i++)
                {
                    graph.AddDependency("s", "" + i);
                }
                Assert.AreEqual(100_000, graph.Size);
            }

            /// <summary>
            /// Check if AddDependency deals with 100,000 dependencies using same child
            /// </summary>
            [TestMethod]
            public void AddDependencyStressTest2()
            {
                DependencyGraph graph = new DependencyGraph();
                for (int i = 0; i < 100_000; i++)
                {
                    graph.AddDependency("" + i, "t");
                }
                Assert.AreEqual(100_000, graph.Size);
            }

            /// <summary>
            /// Check if AddDependency can make 1_000_000 fast enough
            /// </summary>
            [TestMethod]
            public void AddDependencyStressTest3()
            {
                DependencyGraph graph = new DependencyGraph();
                for (int i = 0; i < 1_000_000; i++)
                {
                    graph.AddDependency("s" + i, "t" + i);
                }
                Assert.AreEqual(1_000_000, graph.Size);
            }

            // RemoveDependency Tests

            /// <summary>
            /// Check if RemoveDependency throws a null exception to first argument
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void RemoveDependencyNullCheck()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.RemoveDependency(null, "DG");
            }

            /// <summary>
            /// Check if RemoveDependency throws a null exception to second argument
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void RemoveDependencyNullCheck2()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.RemoveDependency("DG", null);
            }

            /// <summary>
            /// Check if RemoveDependency throws a null exception to both arguments
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void RemoveDependencyNullCheck3()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.RemoveDependency(null, null);
            }

            /// <summary>
            /// Check if RemoveDependency deals with non-existent dependency
            /// </summary>
            [TestMethod]
            public void RemoveDependencyInvalidDependency()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.RemoveDependency("s", "t");
                Assert.AreEqual(0, graph.Size);
            }

            /// <summary>
            /// Check if RemoveDependency removes dependency
            /// </summary>
            [TestMethod]
            public void RemoveDependencyValidDependency()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.AddDependency("s", "t");
                graph.RemoveDependency("s", "t");
                Assert.AreEqual(0, graph.Size);
            }

            /// <summary>
            /// Check if RemoveDependency can remove 100,000 dependencies with same parent
            /// </summary>
            [TestMethod]
            public void RemoveDependencyStressTest1()
            {
                DependencyGraph graph = new DependencyGraph();
                for (int i = 0; i < 100_000; i++)
                {
                    graph.AddDependency("s", "" + i);
                }
                for (int i = 0; i < 100_000; i++)
                {
                    graph.RemoveDependency("s", "" + i);
                }
                Assert.AreEqual(0, graph.Size);
            }

            /// <summary>
            /// Check if RemoveDependency can remove 100,000 dependencies with same child
            /// </summary>
            [TestMethod]
            public void RemoveDependencyStressTest2()
            {
                DependencyGraph graph = new DependencyGraph();
                for (int i = 0; i < 100_000; i++)
                {
                    graph.AddDependency("s" + i, "t");
                }
                for (int i = 0; i < 100_000; i++)
                {
                    graph.RemoveDependency("s" + i, "t");
                }
                Assert.AreEqual(0, graph.Size);
            }

            /// <summary>
            /// Check if RemoveDependency can remove half of 100_000 dependencies and 
            /// ignore non-existent dependencies
            /// </summary>
            [TestMethod]
            public void RemoveDependencyStressTest3()
            {
                DependencyGraph graph = new DependencyGraph();
                for (int i = 0; i < 100_000; i++)
                {
                    graph.AddDependency("s", "" + i);
                }
                for (int i = 0; i < 50_000; i++)
                {
                    graph.RemoveDependency("s", "" + i);
                }
                for (int i = 50_000; i < 100_000; i++)
                {
                    graph.RemoveDependency("s", "" + i + "x");
                }
                Assert.AreEqual(50_000, graph.Size);
            }

            /// <summary>
            /// Check if RemoveDependency can remove 1_000,000 dependencies quickly
            /// </summary>
            [TestMethod]
            public void RemoveDependencyStressTest4()
            {
                DependencyGraph graph = new DependencyGraph();
                for (int i = 0; i < 1_000_000; i++)
                {
                    graph.AddDependency(i + "s", i + "t");
                }
                for (int i = 0; i < 1_000_000; i++)
                {
                    graph.RemoveDependency(i + "s", i + "t");
                }
                Assert.AreEqual(0, graph.Size);
            }

            // ReplaceDependents Tests

            /// <summary>
            /// Check if ReplaceDependents deals throw null exception to first argument
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ReplaceDependentsNullCheck1()
            {
                DependencyGraph graph = new DependencyGraph();
                HashSet<string> newDependents = new HashSet<string>();
                graph.ReplaceDependents(null, newDependents);
            }

            /// <summary>
            /// Check if ReplaceDependents deals throw null exception to second argument
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ReplaceDependentsNullCheck2()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.ReplaceDependents("s", null);
            }

            /// <summary>
            /// Check if ReplaceDependents deals throw null exception to both arguments
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ReplaceDependentsNullCheck3()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.ReplaceDependents(null, null);
            }

            /// <summary>
            /// Check if ReplaceDependents replaces with new dependents
            /// </summary>
            [TestMethod]
            public void ReplaceDependentsValidDependency()
            {
                DependencyGraph graph = new DependencyGraph();
                HashSet<string> newDependents = new HashSet<string>();
                newDependents.Add("f");
                graph.AddDependency("s", "t");
                graph.ReplaceDependents("s", newDependents);
                Assert.AreEqual(1, graph.Size);
            }

            /// <summary>
            /// Check if ReplaceDependents only replaces the same amount of dependents
            /// with the new dependents
            /// </summary>
            [TestMethod]
            public void ReplaceDependentsExtraDependents()
            {
                DependencyGraph graph = new DependencyGraph();
                HashSet<string> newDependents = new HashSet<string>();
                newDependents.Add("c");
                newDependents.Add("a");
                newDependents.Add("k");
                newDependents.Add("e");
                graph.AddDependency("s", "t");
                graph.ReplaceDependents("s", newDependents);
                Assert.AreEqual(1, graph.Size);
            }

            /// <summary>
            /// Check if ReplaceDependents ignores null dependent
            /// </summary>
            [TestMethod]
            public void ReplaceDependentsCheckWhenListContainsNull()
            {
                DependencyGraph graph = new DependencyGraph();
                HashSet<string> newDependents = new HashSet<string>();
                newDependents.Add(null);
                newDependents.Add("x");
                graph.AddDependency("a", "b");
                graph.ReplaceDependents("a", newDependents);
                Assert.AreEqual(1, graph.Size);
            }

            /// <summary>
            /// Check if ReplaceDependents replaces existing children with new ones.
            /// </summary>
            [TestMethod]
            public void ReplaceDependentsStressTest1()
            {
                DependencyGraph graph = new DependencyGraph();
                HashSet<string> newDependents = new HashSet<string>();
                for (int i = 0; i < 100_000; i++)
                {
                    graph.AddDependency("s", "t" + i);
                    newDependents.Add("p" + i);
                }
                graph.ReplaceDependents("s", newDependents);
                HashSet<string> temp = new HashSet<string>(graph.GetDependents("s"));
                for (int i = 0; i < 100_000; i++)
                {
                    Assert.AreEqual(true, temp.Contains("p" + i));
                }
            }

            /// <summary>
            /// ReplaceDependents stress test, but different dependents for first half and second half
            /// </summary>
            [TestMethod]
            public void ReplaceDependentsStressTest2()
            {
                DependencyGraph graph = new DependencyGraph();
                HashSet<string> newDependents = new HashSet<string>();
                HashSet<string> newDependents2 = new HashSet<string>();
                for (int i = 0; i < 50_000; i++)
                {
                    graph.AddDependency("s", "t" + i);
                    newDependents.Add("p" + i);
                }
                graph.ReplaceDependents("s", newDependents);
                for (int i = 50_000; i < 100_000; i++)
                {
                    graph.AddDependency("o", "x" + i);
                    newDependents2.Add("r" + i);
                }
                graph.ReplaceDependents("o", newDependents2);
                HashSet<string> temp = new HashSet<string>(graph.GetDependents("s"));
                HashSet<string> temp2 = new HashSet<string>(graph.GetDependents("o"));
                for (int i = 0; i < 50_000; i++)
                {
                    Assert.AreEqual(true, temp.Contains("p" + i));
                }
                for (int i = 50_000; i < 100_000; i++)
                {
                    Assert.AreEqual(true, temp2.Contains("r" + i));
                }
            }

            /// <summary>
            /// ReplaceDependents stress test, but with a million elements
            /// </summary>
            [TestMethod]
            public void ReplaceDependentsStressTest3()
            {
                DependencyGraph graph = new DependencyGraph();
                HashSet<string> newDependents = new HashSet<string>();
                for (int i = 0; i < 1_000_000; i++)
                {
                    graph.AddDependency("s", "t" + i);
                    newDependents.Add("a" + i);
                }
                graph.ReplaceDependents("s", newDependents);
                HashSet<string> temp = new HashSet<string>(graph.GetDependents("s"));
                for (int i = 0; i < 1_000_000; i++)
                {
                    Assert.AreEqual(true, temp.Contains("a" + i));
                }
            }

            /// <summary>
            /// Check if ReplaceDependee ignores null dependees inside 100_000 elements
            /// and size decreases if null values are inside the list.
            /// </summary>
            [TestMethod]
            public void ReplaceDependentsStressTest4()
            {
                DependencyGraph graph = new DependencyGraph();
                List<string> newDependents = new List<string>();
                for (int i = 0; i < 100_000; i++)
                {
                    if (i % 2 == 0)
                    {
                        newDependents.Add(null);
                    }
                    else
                    {
                        newDependents.Add("a" + i);
                    }
                    graph.AddDependency("c", "b" + i);
                }
                graph.ReplaceDependents("c", newDependents);
                HashSet<string> temp = new HashSet<string>(graph.GetDependents("c"));
                for (int i = 0; i < 50_000; i++)
                {
                    if (i % 2 != 0)
                    {
                        Assert.AreEqual(true, temp.Contains("a" + i));
                    }
                }
                //Assert.AreEqual(50_000, graph.Size);
            }

            // ReplaceDependees Tests

            /// <summary>
            /// Check if ReplaceDependees deals throw null exception to first argument
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ReplaceDependeesNullCheck1()
            {
                DependencyGraph graph = new DependencyGraph();
                List<string> newDependees = new List<string>();
                graph.ReplaceDependees(null, newDependees);
            }

            /// <summary>
            /// Check if ReplaceDependees deals throw null exception to second argument
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ReplaceDependeesNullCheck2()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.ReplaceDependees("s", null);
            }

            /// <summary>
            /// Check if ReplaceDependees deals throw null exception to both arguments
            /// </summary>
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ReplaceDependeesNullCheck3()
            {
                DependencyGraph graph = new DependencyGraph();
                graph.ReplaceDependees(null, null);
            }

            /// <summary>
            /// Check if ReplaceDependees replaces with new dependees
            /// </summary>
            [TestMethod]
            public void ReplaceDependeesValidDependency()
            {
                DependencyGraph graph = new DependencyGraph();
                HashSet<string> newDependees = new HashSet<string>();
                newDependees.Add("f");
                graph.AddDependency("s", "t");
                graph.ReplaceDependees("t", newDependees);
                Assert.AreEqual(1, graph.Size);
            }

            /// <summary>
            /// Check if ReplaceDependees only replaces the same amount of dependees
            /// with the new dependees
            /// </summary>
            [TestMethod]
            public void ReplaceDependeesExtraDependees()
            {
                DependencyGraph graph = new DependencyGraph();
                HashSet<string> newDependees = new HashSet<string>();
                newDependees.Add("c");
                newDependees.Add("a");
                newDependees.Add("k");
                newDependees.Add("e");
                graph.AddDependency("s", "t");
                graph.ReplaceDependees("t", newDependees);
                Assert.AreEqual(1, graph.Size);
            }

            /// <summary>
            /// Check if ReplaceDependee ignores null dependee
            /// </summary>
            [TestMethod]
            public void ReplaceDependeesCheckWhenListContainsNull()
            {
                DependencyGraph graph = new DependencyGraph();
                HashSet<string> newDependees = new HashSet<string>();
                newDependees.Add(null);
                newDependees.Add("x");
                graph.AddDependency("a", "b");
                graph.ReplaceDependents("b", newDependees);
                Assert.AreEqual(1, graph.Size);
            }

            /// <summary>
            /// Check if ReplaceDependees replaces existing parent with new ones.
            /// </summary>
            [TestMethod]
            public void ReplaceDependeesStressTest1()
            {
                DependencyGraph graph = new DependencyGraph();
                HashSet<string> newDependees = new HashSet<string>();
                for (int i = 0; i < 100_000; i++)
                {
                    graph.AddDependency("t" + i, "s");
                    newDependees.Add("p" + i);
                }
                graph.ReplaceDependees("s", newDependees);
                HashSet<string> temp = new HashSet<string>(graph.GetDependees("s"));
                for (int i = 0; i < 100_000; i++)
                {
                    Assert.AreEqual(true, temp.Contains("p" + i));
                }
            }

            /// <summary>
            /// ReplaceDependees stress test, but different dependents for first half and second half
            /// </summary>
            [TestMethod]
            public void ReplaceDependeesStressTest2()
            {
                DependencyGraph graph = new DependencyGraph();
                HashSet<string> newDependees = new HashSet<string>();
                HashSet<string> newDependees2 = new HashSet<string>();
                for (int i = 0; i < 50_000; i++)
                {
                    graph.AddDependency("t" + i, "s");
                    newDependees.Add("z" + i);
                }
                graph.ReplaceDependees("s", newDependees);
                for (int i = 50_000; i < 100_000; i++)
                {
                    graph.AddDependency("x" + i, "o");
                    newDependees2.Add("y" + i);
                }
                graph.ReplaceDependees("o", newDependees2);
                HashSet<string> temp = new HashSet<string>(graph.GetDependees("s"));
                HashSet<string> temp2 = new HashSet<string>(graph.GetDependees("o"));
                for (int i = 0; i < 50_000; i++)
                {
                    Assert.AreEqual(true, temp.Contains("z" + i));
                }
                for (int i = 50_000; i < 100_000; i++)
                {
                    Assert.AreEqual(true, temp2.Contains("y" + i));
                }
            }

            /// <summary>
            /// ReplaceDependees stress test, but with a million elements
            /// </summary>
            [TestMethod]
            public void ReplaceDependeesStressTest3()
            {
                DependencyGraph graph = new DependencyGraph();
                HashSet<string> newDependees = new HashSet<string>();
                for (int i = 0; i < 1_000_000; i++)
                {
                    graph.AddDependency("t" + i, "s");
                    newDependees.Add("r" + i);
                }
                graph.ReplaceDependees("s", newDependees);
                HashSet<string> temp = new HashSet<string>(graph.GetDependees("s"));
                for (int i = 0; i < 1_000_000; i++)
                {
                    Assert.AreEqual(true, temp.Contains("r" + i));
                }
            }

            /// <summary>
            /// Check if ReplaceDependee ignores null dependees inside 100_000 elements
            /// and size decreases if null values are inside the list.
            /// </summary>
            [TestMethod]
            public void ReplaceDependeesStressTest4()
            {
                DependencyGraph graph = new DependencyGraph();
                List<string> newDependees = new List<string>();
                for (int i = 0; i < 100_000; i++)
                {
                    if (i % 2 == 0)
                    {
                        newDependees.Add(null);
                    }
                    else
                    {
                        newDependees.Add("a" + i);
                    }
                    graph.AddDependency("b" + i, "q");
                }
                graph.ReplaceDependees("q", newDependees);
                HashSet<string> temp = new HashSet<string>(graph.GetDependees("q"));
                for (int i = 0; i < 100_000; i++)
                {
                    if (i % 2 != 0)
                    {
                        Assert.AreEqual(true, temp.Contains("a" + i));
                    }
                }
                //Assert.AreEqual(50_000, graph.Size);
            }
        }
    }
}

// Skeleton implementation written by Joe Zachary for CS 3500, January 2018.
// Steen Sia @ February 2, 2018

using System;
using System.Collections.Generic;


namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        // Fields

        // Utilizing Dictionaries which are HashTable/HashMap equivalent in Java which offer constant time for search, insert, and delete
        private Dictionary<String, HashSet<string>> parentList = new Dictionary<string, HashSet<string>>();
        private Dictionary<String, HashSet<string>> childList = new Dictionary<string, HashSet<string>>();

        // Keep track of size
        private int counter;

        /// <summary>
        /// Creates a DependencyGraph containing no dependencies.
        /// </summary>
        public DependencyGraph()
        {
        }

        /// <summary>
        /// Constructor that creates a new constructor by taking in another DependencyGraph and copy its contents
        /// If argument is null, an exception is thrown
        /// </summary>
        /// <param name="otherDependencyGraph"></param>
        public DependencyGraph(DependencyGraph otherDependencyGraph)
        {
            if (otherDependencyGraph == null)
            {
                throw new ArgumentNullException();
            }

            this.parentList = new Dictionary<string, HashSet<string>>();
            this.childList = new Dictionary<string, HashSet<string>>();
            
            foreach (var pair in otherDependencyGraph.parentList)
            {
                this.parentList.Add(pair.Key, new HashSet<string>(otherDependencyGraph.GetDependents(pair.Key)));
            }
            foreach (var pair in otherDependencyGraph.childList)
            {
                this.childList.Add(pair.Key, new HashSet<string>(otherDependencyGraph.GetDependees(pair.Key)));
            }

            this.counter = otherDependencyGraph.counter;
        }

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return counter; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// If s is null, an ArgumentNullException is thrown.
        /// <paramref name="parent"/>
        /// </summary>
        public bool HasDependents(string parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException();
            }

            // Checks if dependee exists, then we know it has a dependent
            if (parentList.ContainsKey(parent))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// If s is null, an ArgumentNullException is thrown.
        /// <paramref name="parent"/>
        /// </summary>
        public bool HasDependees(string parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException();
            }

            // Checks if dependent exists, then we know it has a dependee
            if (childList.ContainsKey(parent))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// If s is null, an ArgumentNullException is thrown.
        /// <paramref name="parent"/>
        /// </summary>
        public IEnumerable<string> GetDependents(string parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException();
            }

            // Return a new HashSet that contained the list of dependents
            if (parentList.ContainsKey(parent))
            {
                return new HashSet<string>(parentList[parent]);
            }

            // If doesn't exist, just return empty enumeration
            return new HashSet<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// If s is null, an ArgumentNullException is thrown.
        /// <paramref name="child"/>
        /// </summary>
        public IEnumerable<string> GetDependees(string child)
        {
            if (child == null)
            {
                throw new ArgumentNullException();
            }

            // Return a new HashSet that contained the list of dependees
            if (childList.ContainsKey(child))
            {
                return new HashSet<string>(childList[child]);
            }

            // If doesn't exist, just return empty enumeration
            return new HashSet<string>();
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// If s and/or t is null, an ArgumentNullException is thrown.
        /// <paramref name="parent"/>
        /// <paramref name="child"/>
        /// </summary>
        public void AddDependency(string parent, string child)
        {
            if (parent == null || child == null)
            {
                throw new ArgumentNullException();
            }

            // Does nothing if user adds duplicate
            if (parentList.ContainsKey(parent) && parentList[parent].Contains(child))
            {
                return;
            }

            // Check if parent exists, then just add child
            if (parentList.ContainsKey(parent))
            {
                // Update the parentList, childList, and size
                AddDependency(1, parentList, childList, parent, child);
                counter++;
            }

            // Check if child exists, then just add parent
            else if (childList.ContainsKey(child))
            {
                //Update the childList, parentList, and size
                AddDependency(2, parentList, childList, parent, child);
                counter++;
            }

            // Create new dependency, update each list and size
            else
            {
                AddDependency(3, parentList, childList, parent, child);
                counter++;
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// If s and/or t is null, an ArgumentNullException is thrown.
        /// <paramref name="parent"/>
        /// <paramref name="child"/>
        /// </summary>
        public void RemoveDependency(string parent, string child)
        {
            if (parent == null || child == null)
            {
                throw new ArgumentNullException();
            }

            // Check if dependency exists
            if (parentList.ContainsKey(parent) && parentList[parent].Contains(child))
            {
                // Remove dependency from parentList and childList
                parentList[parent].Remove(child);
                childList[child].Remove(parent);

                // Removes dependency completely if no links exist with parent or child and update size
                if (parentList[parent].Count < 1)
                {
                    parentList.Remove(parent);
                }
                if (childList[child].Count < 1)
                {
                    childList.Remove(child);
                }
                counter--;
            }

            // Dependency does not exist, simply ignore
            else
            {
                return;
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// If s or IEnumerable newDependents contains a null string,
        /// an ArgumentNullException is thrown.
        /// <paramref name="parent"/>
        /// <paramref name="newDependents"/>
        /// </summary>
        public void ReplaceDependents(string parent, IEnumerable<string> newDependents)
        {
            if (parent == null || newDependents == null)
            {
                throw new ArgumentNullException();
            }

            // Check of the parent exists
            if (parentList.ContainsKey(parent))
            {
                // Remove link with parent for each child
                foreach (string child in parentList[parent])
                {
                    childList[child].Remove(parent);
                    //If there is no link for the child, simply remove
                    if (childList[child].Count < 1)
                    {
                        childList.Remove(child);
                    }
                }

                //Clear all the children for this parent
                parentList[parent].Clear();

                // Add in the new children for this parent
                foreach (string newChild in newDependents)
                {
                    // Ignores null dependents
                    if (newChild == null)
                    {
                        throw new ArgumentNullException();
                    }
                    else
                    {
                        AddDependency(1, parentList, childList, parent, newChild);
                    }
                }
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// If t or IEnumerable newDependees contains a null string,
        /// an ArgumentNullException is thrown.
        /// <paramref name="child"/>
        /// <paramref name="newDependees"/>
        /// </summary>
        public void ReplaceDependees(string child, IEnumerable<string> newDependees)
        {
            if (child == null || newDependees == null)
            {
                throw new ArgumentNullException();
            }

            // Check if child exists
            if (childList.ContainsKey(child))
            {
                // Remove link with child for each parent
                foreach (string parent in childList[child])
                {
                    parentList[parent].Remove(child);
                    if (parentList[parent].Count < 1)
                    {
                        parentList.Remove(parent);
                    }
                }

                // Clear all the parents for this child
                childList[child].Clear();

                // Add in new parents for this child
                foreach (string newParent in newDependees)
                {
                    // Ignores null dependees
                    if (newParent == null)
                    {
                        throw new ArgumentNullException();
                    }
                    else
                    {
                        AddDependency(2, parentList, childList, newParent, child);
                    }
                }
            }
            // If dependee does not exist, create dependency graph for each dependee with t
            else
            {
                foreach (string newParent in newDependees)
                {
                    AddDependency(newParent, child);
                }
            }
        }
        /// <summary>
        /// Private helper method to add a DependencyGraph
        /// The conditions are listed below for when to add
        /// and what to add as a dependency.
        /// <paramref name="condition"/>
        /// <paramref name="parentList"/>
        /// <paramref name="childList"/>
        /// <paramref name="parent"/>
        /// <paramref name="child"/>
        /// </summary>
        private void AddDependency(int condition, Dictionary<string, HashSet<string>> parentList, Dictionary<string, HashSet<string>> childList, string parent, string child)
        {
            switch (condition)
            {
                // Add child if parent already exists
                case 1:
                    parentList[parent].Add(child);
                    if (childList.ContainsKey(child))
                    {
                        childList[child].Add(parent);
                    }
                    else
                    {
                        childList.Add(child, new HashSet<string>());
                        childList[child].Add(parent);
                    }
                    break;

                // Add parent if child already exists
                case 2:
                    childList[child].Add(parent);
                    if (parentList.ContainsKey(parent))
                    {
                        parentList[parent].Add(child);
                    }
                    else
                    {
                        parentList.Add(parent, new HashSet<string>());
                        parentList[parent].Add(child);
                    }
                    break;

                // Create a new dependency graph for parent and child
                case 3:
                    parentList.Add(parent, new HashSet<string>());
                    parentList[parent].Add(child);
                    childList.Add(child, new HashSet<string>());
                    childList[child].Add(parent);
                    break;
            }
        }
    }
}

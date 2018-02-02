// Skeleton implementation written by Joe Zachary for CS 3500, January 2018.
// Steen Sia @ February 1, 2018

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
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return counter; }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            // Checks if dependee exists, then see if it has at least one dependent
            if (parentList.ContainsKey(s) && parentList[s].Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            // Checks if dependent exists, then see if it has at least one dependee
            if (childList.ContainsKey(s) && childList[s].Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            // Return a new HashSet that contained the list of dependents
            if (parentList.ContainsKey(s))
            {
                return new HashSet<string>(parentList[s]);
            }
            // If doesn't exist, just return empty enumeration
            return new HashSet<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException();
            }
            // Return a new HashSet that contained the list of dependees
            if (childList.ContainsKey(s))
            {
                return new HashSet<string>(childList[s]);
            }
            // If doesn't exist, just return empty enumeration
            return new HashSet<string>();
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void AddDependency(string s, string t)
        {
            if (s == null || t == null)
            {
                throw new ArgumentNullException();
            }
            // Does nothing if user adds duplicate
            if (parentList.ContainsKey(s) && childList.ContainsKey(t))
            {
                return;
            }
            // Check if parent exists, then just add child
            if (parentList.ContainsKey(s))
            {
                // Update the parentList, childList, and size
                parentList[s].Add(t);
                childList.Add(t, new HashSet<string>());
                childList[t].Add(s);
                counter++;
            }
            // Check if child exists, then just add parent
            else if (childList.ContainsKey(t))
            {
                // Update the childList, parentList, and size
                childList[t].Add(s);
                parentList.Add(s, new HashSet<string>());
                parentList[s].Add(t);
                counter++;
            }
            // Create new dependency, update each list and size
            else
            {
                parentList.Add(s, new HashSet<string>());
                parentList[s].Add(t);
                childList.Add(t, new HashSet<string>());
                childList[t].Add(s);
                counter++;
            }
        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// </summary>
        public void RemoveDependency(string s, string t)
        {
            if (s == null || t == null)
            {
                throw new ArgumentNullException();
            }
            // Check if dependency exists
            if (parentList.ContainsKey(s) && childList.ContainsKey(t))
            {
                // Remove dependency from parentList and childList
                parentList[s].Remove(t);
                childList[t].Remove(s);
                // Removes dependency completely if no links exist with parent or child and update size
                if (parentList[s].Count < 1)
                {
                    parentList.Remove(s);
                }
                if (childList[t].Count < 1)
                {
                    childList.Remove(t);
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
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s == null || newDependents == null)
            {
                throw new ArgumentNullException();
            }
            // Check of the parent exists
            if (parentList.ContainsKey(s))
            {
                // Remove link with parent for each child
                foreach (string child in parentList[s])
                {
                    childList[child].Remove(s);
                    //If there is no link for the child, simply remove
                    if (childList[child].Count < 1)
                    {
                        childList.Remove(child);
                    }
                }
                //Clear all the children for this parent
                parentList[s].Clear();

                // Keep track of parents to avoid unnecessary links
                int parentCount = 0;

                // Add in the new children for this parent
                foreach (string newChild in newDependents)
                {
                    // Dependencies have been replaced and size remains the same
                    if (counter == parentCount)
                    {
                        break;
                    }
                    // Ignores null dependents
                    if (newChild == null)
                    {
                        counter--;
                    }
                    else
                    { 
                        parentList[s].Add(newChild);
                        childList.Add(newChild, new HashSet<string>());
                        childList[newChild].Add(s);
                        parentCount++;
                    }
                }
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (t == null || newDependees == null)
            {
                throw new ArgumentNullException();
            }
            // Check if child exists
            if (childList.ContainsKey(t))
            {
                // Remove link with child for each parent
                foreach (string parent in childList[t])
                {
                    parentList[parent].Remove(t);
                    if (parentList[parent].Count < 1)
                    {
                        parentList.Remove(parent);
                    }
                }
                // Clear all the parents for this child
                childList[t].Clear();

                // Keep track of children to avoid unnecessary links
                int childCount = 0;

                // Add in new parents for this child
                foreach (string newParent in newDependees)
                {
                    // Dependencies have been replaced and size remains the same 
                    if (counter == childCount)
                    {
                        break;
                    }
                    // Ignores null dependees
                    if (newParent == null)
                    {
                        counter--;
                    }
                    else
                    {
                        childList[t].Add(newParent);
                        parentList.Add(newParent, new HashSet<string>());
                        parentList[newParent].Add(t);
                        childCount++;
                    }
                }
            }
        }
    }
}

/**
 * Simple implementation of the {@link IBTLibrary} interface, which internally
 * uses a Hashtable to map tree names to actual trees. This class also defines
 * methods for adding behaviour trees to the library itself.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System.Collections.Generic;
using DisciplineOak.Execution.Core;
using DisciplineOak.model.Core;

namespace DisciplineOak.Execution.Context
{
	public class GenericBTLibrary : IBTLibrary
	{
		/**
	 * The hashtable that stores all the trees of the library.
	 */
		private readonly Dictionary<string, ModelTask> trees;

		/**
	 * Constructs a GenericBTLibrary containing no trees.
	 */

		public GenericBTLibrary()
		{
			trees = new Dictionary<string, ModelTask>();
		}

		/**
	 * 
	 * @see jbt.execution.core.IBTLibrary#getBT(java.lang.string)
	 */

		public ModelTask getBT(string name)
		{
			return trees[name];
		}

		/**
	 * Returns a read-only iterator through the behaviour trees of the library.
	 * While this iterator is being used, the library cannot be modified.
	 * Otherwise, the results are undefined. Note that both trees and their
	 * names can be accessed through this iterator.
	 * 
	 * @see java.lang.Iterable#iterator()
	 */

		public Dictionary<string, ModelTask> iterator()
		{
			return new Dictionary<string, ModelTask>(trees);
		}

		/**
	 * Adds all the behaviour trees in <code>library</code> to the set of
	 * behaviour trees stored in this library. If there is already a tree with
	 * the same name as that of one of the trees in <code>library</code>, it is
	 * overwritten.
	 * 
	 * @param library
	 *            the library containing all the behaviour trees to add to this
	 *            library.
	 * @return true if a previously stored behaviour tree has been overwritten,
	 *         and false otherwise.
	 */

		public bool addBTLibrary(IBTLibrary library)
		{
			var overwritten = false;

//			foreach (var tuple in library)
//			{
//				
//			}
//
//			for (Tuple<string, ModelTask> tree : library) {
//				if (this.trees.put(tree.getFirst(), tree.getSecond()) != null) {
//					overwritten = true;
//				}
//			}

			return overwritten;
		}

		/**
	 * Adds the behaviour tree <code>tree</code> to the set of behaviour trees
	 * stored in this library. If there is already a tree with the name
	 * <code>name</code>, then it is overwritten by <code>tree</code>.
	 * 
	 * @param name
	 *            the name that will identify the tree <code>tree</code> in the
	 *            library.
	 * @param tree
	 *            the tree to insert.
	 * @return true if there was already a tree with name <code>name</code>, and
	 *         false otherwise.
	 */

		public bool addBT(string name, ModelTask tree)
		{
//			if (this.trees.put(name, tree) != null) {
//				return true;
//			}
//			else {
			return false;
//			}
		}
	}
}
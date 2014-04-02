/**
 * Simple implementation of the {@link IBTLibrary} interface, which internally
 * uses a Hashtable to map tree names to actual trees. This class also defines
 * methods for adding behaviour trees to the library itself.
 * 
 
 * 
 */

using System.Collections.Generic;
using DisciplineOak.Execution.Core;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Execution.Context
{
	public class GenericBTLibrary : IBTLibrary
	{
		/**
	 * The hashtable that stores all the trees of the library.
	 */
		private readonly Dictionary<string, ModelTask> _trees;

		/**
	 * Constructs a GenericBTLibrary containing no trees.
	 */

		public GenericBTLibrary()
		{
			_trees = new Dictionary<string, ModelTask>();
		}

		/**
	 * 
	 * @see jbt.execution.core.IBTLibrary#getBT(java.lang.string)
	 */

		public ModelTask GetBT(string name)
		{
			ModelTask task = null;

			_trees.TryGetValue(name, out task);

			return task;
		}

		public Dictionary<string, ModelTask> GetAllTrees()
		{
			return new Dictionary<string, ModelTask>(_trees);
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

		public bool AddBTLibrary(IBTLibrary library)
		{
			var overwritten = false;

			foreach (var tuple in library.GetAllTrees())
			{
				if(AddBT(tuple.Key, tuple.Value))
					overwritten = true;
			}

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

		public bool AddBT(string name, ModelTask tree)
		{
			bool overwritten = false;
			if (_trees.ContainsKey(name))
			{
				overwritten = true;
				_trees.Remove(name);
			}

			_trees.Add(name, tree);

			return overwritten;
		}
	}
}
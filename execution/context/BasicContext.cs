/**
 * Basic implementation of the IContext interface. This class uses a Dictionary
 * to store the set of variables.
 * <p>
 * Also, since a context must contain a set of behaviour trees, this class
 * defines some methods to add behaviour trees to the context.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using System.Collections.Generic;
using OhBehave.Execution.Core;
using OhBehave.Model.Core;

namespace OhBehave.Execution.context
{
	public class BasicContext : IContext
	{
		private readonly GenericBTLibrary _library;
		private readonly Dictionary<string, object> _variables;

		/**
		 * Default constructor. Constructs an empty BasicContext.
		 */
		public BasicContext()
		{
			_variables = new Dictionary<string, object>();
			_library = new GenericBTLibrary();
		}

		/**
		 * 
		 * @see es.ucm.bt.core.IContext#getVariable(java.lang.string)
		 */
		public Object this[string name]
		{
			get { return _variables[name]; }
		}

		/**
		 * 
		 * @see es.ucm.bt.core.IContext#setVariable(java.lang.string,
		 *      java.lang.Object)
		 */

		public bool SetVariable(string name, Object value)
		{
			if (value == null)
			{
				return _variables.Remove(name);
			}

			if (_variables.ContainsKey(name))
			{
				_variables[name] = value;
				return true;
			}

			_variables.Add(name, value);
			return false;
		}

		/**
	 * 
	 * @see es.ucm.bt.core.IContext#clear()
	 */

		public void Clear()
		{
			_variables.Clear();
		}

		/**
	 * 
	 * @see jbt.execution.core.IContext#clearVariable(java.lang.string)
	 */

		public bool ClearVariable(string name)
		{
			return _variables.Remove(name);
		}

		public ModelTask GetBT(string name)
		{
			return _library.getBT(name);
		}

		/**
	 * Adds all the behaviour trees in <code>library</code> to the set of
	 * behaviour trees stored in the context. If there is already a tree with
	 * the same name as that of one of the trees in <code>library</code>, it is
	 * overwritten.
	 * 
	 * @param library
	 *            the library containing all the behaviour trees to add to this
	 *            context.
	 * @return true if a previously stored behaviour tree has been overwritten,
	 *         and false otherwise.
	 */

		public bool AddBTLibrary(IBTLibrary library)
		{
			return _library.addBTLibrary(library);
		}

		/**
	 * Adds the behaviour tree <code>tree</code> to the set of behaviour trees
	 * stored in the context. If there is already a tree with the name
	 * <code>name</code>, then it is overwritten by <code>tree</code>.
	 * 
	 * @param name
	 *            the name that will identify the tree <code>tree</code> in the
	 *            context.
	 * @param tree
	 *            the tree to insert.
	 * @return true if there was already a tree with name <code>name</code>, and
	 *         false otherwise.
	 */

		public bool AddBT(string name, ModelTask tree)
		{
			return _library.addBT(name, tree);
		}
	}
}
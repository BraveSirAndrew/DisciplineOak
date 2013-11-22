/**
 * A HierarchicalContext is a context that stores a parent IContext to fall back
 * to when it cannot find a particular variable in its own set of variables.
 * This class just redefines the method {@link #getVariable(string)} so that if
 * the variable name cannot be found, its value is retrieved from the parent
 * context.
 * 
 
 * 
 */

using System;
using DisciplineOak.Execution.Core;

namespace DisciplineOak.Execution.Context
{
	public class HierarchicalContext : BasicContext
	{
		/**
		 * The parent context. When a variable cannot be retrieved from the current
		 * context, it will be looked up in the parent context.
		 */
		private IContext _parent;
		
		/**
		 * Sets the parent context of this HierarchicalContext. May be null, in
		 * which case no parent context will be used.
		 * 
		 * @param parent
		 *            the parent context, which may be null.
		 */
		public void SetParent(IContext parent)
		{
			_parent = parent;
		}

		/**
		 * Returns the value of a variable whose name is <code>name</code>. If a
		 * variable with such a name cannot be found in the current context, and
		 * there is a parent context set, the variable will be looked up in the
		 * parent context, and its value returned. If it cannot be found in the
		 * parent context (or in any other parent contexts through recursion), null
		 * is returned.
		 */

		public override object this[string name]
		{
			get
			{
				var result = base[name];

				if (result != null) 
					return result;

				if (_parent != null)
				{
					result = _parent[name];
				}

				return result;
			}
		}
	}
}
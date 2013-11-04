/**
 * The SafeContext represents a context that can be used to safely controls
 * modifications in another context (the <i>input context</i>).
 * <p>
 * A SafeContext contains an IContext (the <i>input context</i>). Initially, all
 * variables are read form the input context. However, when a variable is set or
 * cleared, its value is not modified in the input context, but it is locally
 * modified instead. From then on, the variable will be locally read instead of
 * reading if from the input context. Thus, the input context is never modified.
 * <p>
 * SafeContext can be used to situations in which an entity should use another
 * context (the input context) in read-only mode. If such entity uses a
 * SafeContext, it will not modify the input context, but on the other hand will
 * interact with the SafeContext in just the same way it would with the input
 * context.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using System.Collections.Generic;
using DisciplineOak.Execution.Core;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Execution.Context
{
	public class SafeContext : IContext
	{
		/**
	 * The original input context which the SafeOutputContext is based on.
	 */
		private readonly IContext inputContext;
		/**
	 * Flag that tells whether the SafeOutputContext has been cleared.
	 */
		/**
	 * Set containing the names of those variables whose value has been set or
	 * cleared by the SafeOutputContext.
	 */
		private readonly List<string> localModifiedVariables;
		private readonly Dictionary<string, Object> localVariables;
		private bool cleared;

		/**
	 * Constructs a SafeContext whose input context is <code>inputContext</code>
	 * .
	 * 
	 * @param inputContext
	 *            the input context.
	 */

		public SafeContext(IContext inputContext)
		{
			this.inputContext = inputContext;
			localVariables = new Dictionary<string, Object>();
			cleared = false;
			localModifiedVariables = new List<string>();
		}

		/**
		 * Retrieves the value of a variable.If the variable has not been modified
		 * by the SafeContext, its value is retrieved from the input context.
		 * However, if the variable has been modified (either cleared or set), the
		 * value will be retrieved from the SafeContext.
		 * 
		 * @param name
		 *            the name of the variable to retrieve.
		 * 
		 * @return the value of a variable whose name is <code>name</code>, or null
		 *         if it does not exist.
		 * 
		 */
		public Object this[string name]
		{
			get
			{
				if (localModifiedVariables.Contains(name) || cleared)
				{
					return localVariables[name];
				}
				var variable = localVariables[name];
				return variable ?? inputContext[name];
			}
		}

		/**
		 * Sets the value of a variable. Its value is not written into the input
		 * context. Instead, its value is stored into a local variable managed by
		 * the SafeContext.
		 * 
		 * @param name
		 *            the name of the variable.
		 * @param value
		 *            the value for the variable.
		 * @return true if a variable with the same name already existed, and false
		 *         otherwise.
		 */
		public bool SetVariable(string name, Object value)
		{
			if (!localModifiedVariables.Contains(name))
			{
				localModifiedVariables.Add(name);
			}
			if (value == null)
			{
				return localVariables.Remove(name);
			}

			if (localVariables.ContainsKey(name))
			{
				localVariables[name] = value;
				return true;
			}

			localVariables.Add(name, value);
			return true;
		}

		/**
		 * Clears the context. Variables are not removed from the input context, but
		 * from the set of local variables managed by the SafeContext.
		 * 
		 */

		public void Clear()
		{
			localVariables.Clear();
			cleared = true;
		}

		/**
		 * Clears a variable of the context. If it not removed from the input
		 * context, but from the set of local variables managed by the SafeContext.
		 * 
		 * @param name
		 *            the name of the variable to clear.
		 * @return true if a variable was actually cleared, and false in case it did
		 *         not exist.
		 * 
		 */
		public bool ClearVariable(string name)
		{
			if (!localModifiedVariables.Contains(name))
			{
				localModifiedVariables.Add(name);
			}
			return localVariables.Remove(name);
		}

		/**
		 * Returns the behaviour tree of a particular name, or null in case it
		 * cannot be found. The behaviour tree is extracted from the input context
		 * passed at construction time.
		 * 
		 * @param the
		 *            name of the behaviour tree to retrieve.
		 * @return the behaviour tree, or null if it cannot be found.
		 * 
		 * @see jbt.execution.core.IContext#getBT(java.lang.string)
		 */
		public ModelTask GetBT(string name)
		{
			return inputContext.GetBT(name);
		}
	}
}
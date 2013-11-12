/**
 * The SafeOutputContext represents a context that can be used to safely
 * controls modifications in another context (the <i>input context</i>).
 * <p>
 * A SafeOutputContext contains an IContext (the <i>input context</i>), and a
 * list of <i>output variables</i>. These are the variables that can be written
 * into the input context. The rest of variables are stored locally in the
 * SafeOutputContext.
 * <p>
 * Thus, when the SafeOutputContext sets the value of a variable, it will
 * normally set its value in a local variable. However, if the variable is one
 * of the list of output variables, the value will be set in the input context.
 * <p>
 * When retrieving variables, a variable in the list of output variables will
 * always be retrieved from the input context. A variable that is not in the
 * list of output variables will also be retrieved from the input context;
 * however, when such variable is modified (either its value is changed or
 * cleared), the value will be retrieved from the SafeOutputContext (that is,
 * from the moment a variable that is not in the list of output variables is
 * modified, it is managed locally).
 * <p>
 * With respect to clearing variables, output variables are always cleared in
 * the input context. However, since only output variables can be modified in
 * the input context, any other variable will be cleared in the
 * SafeOutputContext.
 * <p>
 * The SafeOutputContext can be used in situations where an entity must use a
 * context (the input context) in a soft-read-only mode. By using the
 * SafeOutputContext, such entity will only be able to modify the output
 * variables in the input context. On the other hand, it will interact with the
 * SafeOutputContext in just the same way it would with the input context.
 * 
 
 * 
 */

using System;
using System.Collections.Generic;
using DisciplineOak.Execution.Core;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Execution.Context
{
	public class SafeOutputContext : IContext
	{
		/**
	 * The original input context which the SafeOutputContext is based on.
	 */
		private readonly IContext inputContext;
		/**
	 * The list of output variables. These variables can be written into the
	 * {@link #inputContext}, unlike the rest, that are stored in
	 * {@link #localVariables}.
	 */
		/**
	 * Set containing the names of those non-output variables whose value has
	 * been set or cleared by the SafeOutputContext.
	 */
		private readonly List<string> localModifiedVariables;
		/**
	 * Flag that tells whether the SafeOutputContext has been cleared.
	 */
		/**
	 * The set of local variables managed by the SafeOutputContext.
	 */
		private readonly Dictionary<string, Object> localVariables;
		private readonly List<string> outputVariables;
		private bool cleared;

		/**
	 * Constructs a SafeOutputContext whose input context is
	 * <code>inputContext</code> and whose list of output variables is
	 * <code>outputVariables</code>.
	 * 
	 * @param inputContext
	 *            the input context.
	 * @param outputVariables
	 *            the list of output variables.
	 */

		public SafeOutputContext(IContext inputContext, List<string> outputVariables)
		{
			this.inputContext = inputContext;
			this.outputVariables = outputVariables;
			localVariables = new Dictionary<string, Object>();
			localModifiedVariables = new List<string>();
			cleared = false;
		}

		/**
	 * Retrieves the value of a variable. If it is an output variable, its value
	 * is retrieved from the input context. Otherwise, if the variable has not
	 * been modified by the SafeOutputContext, its value is also retrieved from
	 * the input context. However, if the variable has been modified (either
	 * cleared or set), the value will be retrieved from the SafeOutputContext.
	 * 
	 * @param name
	 *            the name of the variable to retrieve.
	 * 
	 * @return the value of a variable whose name is <code>name</code>, or null
	 *         if it does not exist.
	 * 
	 * @see jbt.execution.core.IContext#getVariable(java.lang.string)
	 */

		public Object this[string name]
		{
			get
			{
				if (outputVariables.Contains(name))
				{
					return inputContext[name];
				}
				if (localModifiedVariables.Contains(name) || cleared)
				{
					return localVariables[name];
				}
				var variable = localVariables[name];
				if (variable != null)
				{
					return variable;
				}
				return inputContext[name];
			}
		}

		/**
	 * Sets the value of a variable. If it is an output variable, its value is
	 * written into the input context. Otherwise, its value will be stored into
	 * a local variable managed by the SafeOutputContext.
	 * 
	 * @param name
	 *            the name of the variable.
	 * @param value
	 *            the value for the variable.
	 * @return true if a variable with the same name already existed, and false
	 *         otherwise.
	 * 
	 * @see jbt.execution.core.IContext#setVariable(java.lang.string,
	 *      java.lang.Object)
	 */

		public bool SetVariable(string name, Object value)
		{
			if (outputVariables.Contains(name))
			{
				return inputContext.SetVariable(name, value);
			}
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
	 * Clears the context. Output variables are cleared in the input context.
	 * The rest are removed from the set of local variables managed by the
	 * SafeOutputContext.
	 * 
	 * @see jbt.execution.core.IContext#clear()
	 */

		public void Clear()
		{
			localVariables.Clear();

			foreach (var outputVariable in outputVariables)
			{
				inputContext.ClearVariable(outputVariable);
			}

			cleared = true;
		}

		/**
	 * Clears a variable of the context. If it is an output variable, the value
	 * is cleared in the input context. Otherwise, the variable is removed from
	 * the set of local variables managed by the SafeOutputContext.
	 * 
	 * @param name
	 *            the name of the variable to clear.
	 * @return true if a variable was actually cleared, and false in case it did
	 *         not exist.
	 * 
	 * @see jbt.execution.core.IContext#clearVariable(java.lang.string)
	 */

		public bool ClearVariable(string name)
		{
			if (outputVariables.Contains(name))
			{
				return inputContext.ClearVariable(name);
			}
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
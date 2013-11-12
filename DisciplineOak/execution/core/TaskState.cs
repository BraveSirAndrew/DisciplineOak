/**
 * Default implementation of the {@link ITaskState} interface. It provides
 * methods for modifying the set of variables stored by the TaskState.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using System.Collections.Generic;

namespace DisciplineOak.Execution.Core
{
	public class TaskState : ITaskState
	{
		/** The set of variables. */
		private readonly Dictionary<string, Object> variables;

		/**
	 * Constructs an empty TaskState.
	 */

		public TaskState()
		{
			variables = new Dictionary<string, Object>();
		}


		public Object GetStateVariable(string name)
		{
			return variables[name];
		}

		/**
	 * Sets the value of a variable. If the value is null, the variable is
	 * cleared.
	 * 
	 * @param name
	 *            the name of the variable.
	 * @param value
	 *            the value of the variable.
	 * @return true if there was a variable with name <code>name</code> before
	 *         calling this method (it is therefore been overwritten), and false
	 *         otherwise.
	 */

		public bool setStateVariable(string name, Object value)
		{
			if (value == null)
			{
				return variables.Remove(name);
			}

			if (variables.ContainsKey(name))
			{
				variables[name] = value;
				return false;
			}

			variables.Add(name, value);
			return true;
		}

		/**
	 * Clears all the variables of the TaskState.
	 */

		public void clear()
		{
			variables.Clear();
		}

		/**
	 * Clears the value of a variable.
	 * 
	 * @param name
	 *            the name of the variable.
	 * @return true if the variable existed before calling this method, and
	 *         false otherwise.
	 */

		public bool clearStateVariable(string name)
		{
			return variables.Remove(name);
		}
	}
}
/**
 * Default implementation of the {@link ITaskState} interface. It provides
 * methods for modifying the set of variables stored by the TaskState.
 * 
 */
using System;
using System.Collections.Generic;

namespace DisciplineOak.Execution.Core
{
	public class TaskState : ITaskState
	{
		
		private readonly Dictionary<string, Object> _variables;
		
		public TaskState()
		{
			_variables = new Dictionary<string, Object>();
		}
		
		public Object GetStateVariable(string name)
		{
			return _variables[name];
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

		public bool SetStateVariable(string name, Object value)
		{
			if (value == null)
			{
				return _variables.Remove(name);
			}

			if (_variables.ContainsKey(name))
			{
				_variables[name] = value;
				return false;
			}

			_variables.Add(name, value);
			return true;
		}

		/**
	 * Clears all the variables of the TaskState.
	 */

		public void Clear()
		{
			_variables.Clear();
		}

		/**
	 * Clears the value of a variable.
	 * 
	 * @param name
	 *            the name of the variable.
	 * @return true if the variable existed before calling this method, and
	 *         false otherwise.
	 */
		public bool ClearStateVariable(string name)
		{
			return _variables.Remove(name);
		}
	}
}
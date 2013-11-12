/**
 * The TaskStateFactory implements the simple factory pattern, and allows
 * clients of the framework to create instances of {@link ITaskState} objects.
 * The methods provided by this factory allows the client to specify the set of
 * variables that the task state will contain.
 * 
 
 * 
 */

using System;
using System.Collections.Generic;

namespace DisciplineOak.Execution.Core
{
	public class TaskStateFactory
	{
		/**
	 * Creates an ITaskState that contains the set of variables specified by
	 * <code>variables</code>. Each variable is a Tuple whose first element is
	 * the variable's name and the second element is its value.
	 * 
	 * @param variables
	 *            the list of variables that the ITaskState will contain.
	 * @return an ITaskState that contains the set of variables in
	 *         <code>variables</code>.
	 */

		public static ITaskState CreateTaskState(List<Tuple<string, Object>> variables)
		{
			var taskState = new TaskState();

			foreach (var variable in variables)
			{
				taskState.SetStateVariable(variable.Item1, variable.Item2);
			}

			return taskState;
		}

		/**
	 * Creates an ITaskState that contains the set of variables specified by
	 * <code>variables</code>. Variables are stored in a Map whose keys are
	 * variables' names and whose values are the values of the variables.
	 * 
	 * @param variables
	 *            the list of variables that the ITaskState will contain.
	 * @return an ITaskState that contains the set of variables in
	 *         <code>variables</code>.
	 */

		public static ITaskState CreateTaskState(Dictionary<string, Object> variables)
		{
			var taskState = new TaskState();

			foreach (var variable in variables)
			{
				taskState.SetStateVariable(variable.Key, variable.Value);
			}

			return taskState;
		}
	}
}
/**
 * The ITaskState interface represents the persistent state of a task in a
 * behaviour tree. This state is represented as a set of variables with name and
 * value.
 * <p>
 * Some tasks in BTs are persistent in the sense that, after finishing, if they
 * are spawned again, they remember past information. Take for example the
 * "limit" task. A "limit" task allows to run its child node only a certain
 * number of times (for example, 5). After being spawned, it has to remember how
 * many times it has been run so far, so that, once the threshold is exceeded,
 * it fails.
 * <p>
 * This interface represents the common functionality for classes that represent
 * the persistent state of a task. It just defines a method for retrieving the
 * value of a variable of the task's state. The way the task's state is
 * populated is not defined.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;

namespace DisciplineOak.Execution.Core
{
	public interface ITaskState
	{
		/**
		 * Returns the value of a variable whose name is <code>name</code>, or null
		 * if it is not found.
		 * 
		 * @param name
		 *            the name of the variable to retrieve.
		 * 
		 * @return the value of a variable whose name is <code>name</code>, or null
		 *         if it does not exist.
		 */
		Object GetStateVariable(string name);
	}
}
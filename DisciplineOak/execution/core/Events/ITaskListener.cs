/**
 * Interface for an entity that is able to receive events from tasks (
 * {@link ExecutionTask}) whose status has changed in a relevant way.
 * 
 
 * 
 */

namespace DisciplineOak.Execution.Core.Events
{
	public interface ITaskListener
	{
		/**
	 * Method called when an important change in the status of a task has taken
	 * place.
	 * 
	 * @param e
	 *            the TaskEvent with all the information about the change in the
	 *            status of the task.
	 */
		void StatusChanged(TaskEvent e);
	}
}
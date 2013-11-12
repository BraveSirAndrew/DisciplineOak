/**
 * Interface for an entity that is able to receive events from tasks (
 * {@link ExecutionTask}) whose status has changed in a relevant way.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

namespace DisciplineOak.Execution.Core.@event
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
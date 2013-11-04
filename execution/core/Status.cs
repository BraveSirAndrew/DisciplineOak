namespace OhBehave.Execution.Core
{
	/**
	 * Enum defining the possible states of an ExecutionTask. Throughout its
	 * execution, an ExecutionTask may be in several states:
	 * 
	 * <ul>
	 * <li> {@link #FAILURE}: means the task has failed, that is, it could not
	 * complete successfully.
	 * <li> {@link #SUCCESS}: means the task has completed successfully.
	 * <li> {@link #RUNNING}: means the task is still running.
	 * <li> {@link #TERMINATED}: means the task has been abruptly terminated. It
	 * is conceptually similar to {@link #FAILURE}, so whenever a task has been
	 * terminated, it is also considered to have failed.
	 * <li> {@link #UNINITIALIZED}: means the task has not been spawned yet, that
	 * is, it has not started executing.
	 * </ul>
	 * 
	 * @author Ricardo Juan Palma Durán
	 * 
	 */
	public enum Status
	{
		/** Status code meaning the task has failed. */
		Failure, /** Status code meaning the task has succeeded. */
		Success, /** Status code meaning the task is still running. */
		Running, /**
		 * Status code meaning the task has been abruptly terminated.
		 * It is conceptually similar to {@link #FAILURE}, so whenever a task
		 * has been terminated, it is also considered to have failed.
		 */
		Terminated,
		/**
		 * Status code meaning the task has not been spawned yet, that is, it
		 * has not started executing.
		 */
		Uninitialized,
	}
}
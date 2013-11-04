/**
 * ExecutionInverter is the ExecutionTask that knows how to run a ModelInverter.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using OhBehave.Execution.Core;
using OhBehave.Execution.core;
using OhBehave.execution.core;
using OhBehave.Model.Core;
using OhBehave.Model.Task.decorator;

namespace OhBehave.Execution.Task.decorator
{
	public class ExecutionInverter : ExecutionDecorator
	{
		/** The child. */
		private ExecutionTask child;

		/**
	 * Creates an ExecutionInverter that is able to run a ModelInverter task and
	 * that is managed by a BTExecutor.
	 * 
	 * @param modelTask
	 *            the ModelInverter that this ExecutionInverter is going to run.
	 * @param executor
	 *            the BTExecutor in charge of running this ExecutionInverter.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

		public ExecutionInverter(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelInverter))
			{
				throw new ArgumentException("The ModelTask must subclass ModelInverter but it inherits from " +
				                            modelTask.GetType().Name);
			}
		}

		/**
	 * Spawns the only child.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalSpawn()
	 */

		protected override void InternalSpawn()
		{
			/* Just spawn the only child. */
			child = ((ModelInverter) ModelTask).getChild().CreateExecutor(
				Executor, this);
			child.AddTaskListener(this);
			child.Spawn(Context);
		}

		/**
	 * Terminates the only child.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTerminate()
	 */

		protected override void InternalTerminate()
		{
			/* Just terminates the only child. */
			child.Terminate();
		}

		/**
	 * Checks if the only child has already finished. If so, it inverts its
	 * status code. Otherwise, it returns {@link Status#RUNNING}.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTick()
	 */

		protected override Status InternalTick()
		{
			/* Just inverts the status code. */
			Status childStatus = child.GetStatus();
			if (childStatus == Status.Running)
			{
				return Status.Running;
			}
			if (childStatus == Status.Failure || childStatus == Status.Terminated)
			{
				return Status.Success;
			}
			return Status.Failure;
		}

		/**
	 * Does nothing.
	 * 
	 * @see jbt.execution.core.ExecutionTask#restoreState(ITaskState)
	 */

		protected override void RestoreState(ITaskState state)
		{
		}

		/**
	 * Just calls {@link #tick()} so that the ExecutionInverter can evolve
	 * according to the termination of its child.
	 * 
	 * @see jbt.execution.core.ExecutionTask#statusChanged(jbt.execution.core.event.TaskEvent)
	 */

		public override void StatusChanged(TaskEvent e)
		{
			Tick();
		}

		/**
	 * Does nothing.
	 * 
	 * @see jbt.execution.core.ExecutionTask#storeState()
	 */

		protected override ITaskState StoreState()
		{
			return null;
		}

		/**
	 * Does nothing.
	 * 
	 * @see jbt.execution.core.ExecutionTask#storeTerminationState()
	 */

		protected override ITaskState StoreTerminationState()
		{
			return null;
		}
	}
}
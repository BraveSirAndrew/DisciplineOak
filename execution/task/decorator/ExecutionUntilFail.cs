/**
 * ExecutionUntilFail is the ExecutionTask that knows how to run a
 * ModelUntilFail.
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
	public class ExecutionUntilFail : ExecutionDecorator
	{
		/** The task that is being decorated. */
		private ExecutionTask child;

		/**
	 * Constructs and ExecutionUntilFail that knows how to run a ModelUntilFail.
	 * 
	 * @param modelTask
	 *            the ModelUntilFail that this ExecutionUntilFail will run.
	 * @param executor
	 *            the BTExecutor that will manage this ExecutionUntilFail.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

		public ExecutionUntilFail(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelUntilFail))
			{
				throw new ArgumentException("The ModelTask must subclass ModelUntilFail but it inherits from " +
				                            modelTask.GetType().Name);
			}
		}

		/**
	 * Spawns the child task.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalSpawn()
	 */

		protected override void InternalSpawn()
		{
			child = ((ModelDecorator) ModelTask).getChild().CreateExecutor(
				Executor, this);
			child.AddTaskListener(this);
			child.Spawn(Context);
		}

		/**
	 * Just terminates the child of this task.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTerminate()
	 */

		protected override void InternalTerminate()
		{
			child.Terminate();
		}

		/**
	 * If the child has finished in failure or been terminated, return
	 * {@link Status#SUCCESS}. Otherwise, {@link Status#RUNNING} is returned. If
	 * the child has finished successfully, it is spawned again.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTick()
	 */

		protected override Status InternalTick()
		{
			Status childStatus = child.GetStatus();

			/*
		 * If the child has finished in failure or been terminated, return
		 * success.
		 */
			if (childStatus == Status.Failure || childStatus == Status.Terminated)
			{
				return Status.Success;
			}
			/* If the child has finished successfully, spawn it again. */
			if (childStatus == Status.Success)
			{
				child = ((ModelDecorator) ModelTask).getChild().CreateExecutor(
					Executor, this);
				child.AddTaskListener(this);
				child.Spawn(Context);
			}

			/*
			 * In case the child has not finished in failure, return
			 * Status.RUNNING.
			 */
			return Status.Running;
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
	 * Just calls {@link #tick()} to make the task evolve.
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
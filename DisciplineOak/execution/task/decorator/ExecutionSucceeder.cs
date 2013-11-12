/**
 * ExecutionSucceeder is the ExecutionTask that knows how to run a ModelSucceeder.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Core.@event;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.decorator;

namespace DisciplineOak.Execution.Task.Decorator
{
	public class ExecutionSucceeder : ExecutionDecorator
	{
		/** The child that is being decorated. */
		private ExecutionTask child;

		/**
	 * Creates an ExecutionSucceeder that knows how to run a ModelSucceeder.
	 * 
	 * @param modelTask
	 *            the ModelSucceeder to run.
	 * @param executor
	 *            the BTExecutor that will manage this ExecutionSucceeder.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

		public ExecutionSucceeder(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelSucceeder))
			{
				throw new ArgumentException("The ModelTask must subclass ModelSucceeder but it inherits from " +
				                            modelTask.GetType().Name);
			}
		}

		/**
	 * Just spawns its child.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalSpawn()
	 */

		protected override void InternalSpawn()
		{
			child = ((ModelSucceeder) ModelTask).getChild().CreateExecutor(
				Executor, this);

			child.AddTaskListener(this);
			child.Spawn(Context);
		}

		/**
	 * Just ticks its child.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTick()
	 */

		protected override Status InternalTick()
		{
			Status childStatus = child.GetStatus();

			if (childStatus == Status.Running)
			{
				return Status.Running;
			}

			return Status.Success;
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

		/**
	 * Does nothing.
	 * 
	 * @see jbt.execution.core.ExecutionTask#restoreState(jbt.execution.core.ITaskState)
	 */

		protected override void RestoreState(ITaskState state)
		{
		}

		/**
	 * Just ticks the task.
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
	 * @see jbt.execution.core.ExecutionTask#internalTerminate()
	 */

		protected override void InternalTerminate()
		{
			child.Terminate();
		}
	}
}
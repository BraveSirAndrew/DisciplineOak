/**
 * ExecutionSafeContextManager is the ExecutionTask that knows how to run a
 * ModelSafeContextManager.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using OhBehave.Execution.context;
using OhBehave.Execution.Core;
using OhBehave.Execution.core;
using OhBehave.execution.core;
using OhBehave.Model.Core;
using OhBehave.Model.Task.decorator;

namespace OhBehave.Execution.Task.decorator
{
	public class ExecutionSafeContextManager : ExecutionDecorator
	{
		/** The child task. */
		private ExecutionTask child;

		/**
	 * Constructs an ExecutionSafeContextManager that knows how to run a
	 * ModelSafeContextManager.
	 * 
	 * @param modelTask
	 *            the ModelSafeContextManager to run.
	 * @param executor
	 *            the BTExecutor that will manage this
	 *            ExecutionSafeContextManager.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

		public ExecutionSafeContextManager(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelSafeContextManager))
			{
				throw new ArgumentException("The ModelTask must subclass ModelSafeContextManager but it inherits from " +
				                            modelTask.GetType().Name);
			}
		}

		/**
	 * Spawns the child task. This method creates a new SafeContext, and spawns
	 * the child task using this SafeContext. The input context of the
	 * SafeContext is that of this ExecutionSafeContextManager task.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalSpawn()
	 */

		protected override void InternalSpawn()
		{
			var newContext = new SafeContext(Context);
			child = ((ModelDecorator) ModelTask).getChild().CreateExecutor(
				Executor, this);
			child.AddTaskListener(this);
			child.Spawn(newContext);
		}

		/**
	 * Just terminates the child task.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTerminate()
	 */

		protected override void InternalTerminate()
		{
			child.Terminate();
		}

		/**
	 * Returns the current status of the child.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTick()
	 */

		protected override Status InternalTick()
		{
			return child.GetStatus();
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
	 * Just calls {@link #tick()} to make the tass evolve.
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
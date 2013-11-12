/**
 * ExecutionHierarchicalContextManager is the ExecutionTask that knows how to run a
 * ModelHierarchicalContextManager.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using DisciplineOak.Execution.Context;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Core.@event;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.decorator;

namespace DisciplineOak.Execution.Task.Decorator
{
	public class ExecutionHierarchicalContextManager : ExecutionDecorator
	{
		/** The child task. */
		private ExecutionTask child;

		/**
	 * Constructs an ExecutionHierarchicalContextManager that knows how to run a
	 * ModelHierarchicalContextManager.
	 * 
	 * @param modelTask
	 *            the ModelHierarchicalContextManager to run.
	 * @param executor
	 *            the BTExecutor that will manage this ExecutionHierarchicalContextManager.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

		public ExecutionHierarchicalContextManager(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelHierarchicalContextManager))
			{
				throw new ArgumentException("The ModelTask must subclass ModelHierarchicalContextManager but it inherits from " +
				                            modelTask.GetType().Name);
			}
		}

		/**
	 * Spawns the child task. This method creates a new HierarchicalContext,
	 * sets its parent to the context of the ExecutionHierarchicalContextManager, and spawns
	 * the child task using this HierarchicalContext.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalSpawn()
	 */

		protected override void InternalSpawn()
		{
			var newContext = new HierarchicalContext();
			newContext.SetParent(Context);
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
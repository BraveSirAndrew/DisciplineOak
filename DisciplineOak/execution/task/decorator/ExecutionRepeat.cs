/**
 * ExecutionRepeat is the ExecutionTask that knows how to run a ModelForever.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Core.@event;
using DisciplineOak.model.Core;
using DisciplineOak.model.Task.Decorator;

namespace DisciplineOak.Execution.Task.decorator
{
	public class ExecutionRepeat : ExecutionDecorator
	{
		/** The child task. */
		private ExecutionTask _child;

			/**
		 * Constructs an ExecutionRepeat that knows how to run a ModelForever.
		 * 
		 * @param modelTask
		 *            the ModelForever to run.
		 * @param executor
		 *            the BTExecutor that will manager this ExecutionRepeat.
		 * @param parent
		 *            the parent ExecutionTask of this task.
		 */
		public ExecutionRepeat(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelRepeat))
			{
				throw new ArgumentException("The ModelTask must subclass ModelRepeat but it inherits from " +
				                            modelTask.GetType().Name);
			}
		}

		/**
	 * Just spawns its child task.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalSpawn()
	 */

		protected override void InternalSpawn()
		{
			_child = ((ModelRepeat) ModelTask).getChild().CreateExecutor(Executor, this);
			_child.AddTaskListener(this);
			_child.Spawn(Context);
		}

		/**
		 * Terminates the child task.
		 * 
		 * @see jbt.execution.core.ExecutionTask#internalTerminate()
		 */
		protected override void InternalTerminate()
		{
			_child.Terminate();
		}

		/**
		 * If the child task has finished, it spawns it again. Always returns
		 * {@link Status#RUNNING}.
		 * 
		 * @see jbt.execution.core.ExecutionTask#internalTick()
		 */

		protected override Status InternalTick()
		{
			Status childStatus = _child.GetStatus();

			/*
		 * If the child has finished, spawn it again
		 */
			if (childStatus != Status.Running)
			{
				_child = ((ModelDecorator) ModelTask).getChild().CreateExecutor(Executor, this);
				_child.AddTaskListener(this);
				_child.Spawn(Context);
			}

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
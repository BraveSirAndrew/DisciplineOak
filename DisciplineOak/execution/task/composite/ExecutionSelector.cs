/**
 * ExecutionSelector is the ExecutionTask that is able to run a ModelSelector
 * task.
 * 
 
 * 
 */

using System;
using System.Collections.Generic;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Core.Events;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.Composite;

namespace DisciplineOak.Execution.Task.Composite
{
	public class ExecutionSelector : ExecutionComposite
	{
		/** Index of the active child. */
		/** The currently active child. */
		private ExecutionTask _activeChild;
		private int _activeChildIndex;
		/** List of the ModelTask children of the selector. */
		private List<ModelTask> _children;

		/**
	 * Creates an ExecutionSelector that is able to run a ModelSelector task and
	 * that is managed by a BTExecutor.
	 * 
	 * @param modelTask
	 *            the ModelSelector that this ExecutionSelector is going to run.
	 * @param executor
	 *            the BTExecutor in charge of running this ExecutionSelector.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

		public ExecutionSelector(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelSelector))
			{
				throw new ArgumentException("The ModelTask must subclass " + typeof (ModelSelector).Name + " but it inherits from " +
				                            modelTask.GetType().Name);
			}
		}

		/**
	 * Spawns the first child.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalSpawn()
	 */
		protected override void InternalSpawn()
		{
			_activeChildIndex = 0;
			_children = ModelTask.Children;
			_activeChild = _children[_activeChildIndex].CreateExecutor(Executor, this);
			_activeChild.AddTaskListener(this);
			_activeChild.Spawn(Context);
		}

		/**
	 * Terminates the current active child.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTerminate()
	 */

		protected override void InternalTerminate()
		{
			_activeChild.Terminate();
		}

		/**
	 * Checks if the currently active child has finished. It it has not, it
	 * returns {@link Status#RUNNING}. If it has finished successfully, it
	 * returns {@link Status#SUCCESS}. If it has finished in failure, then:
	 * <ul>
	 * <li>If it was the last child of the selector, returns
	 * {@link Status#FAILURE}.
	 * <li>Otherwise, it spawns the next child of the selector and returns
	 * {@link Status#RUNNING}.
	 * </ul>
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTick()
	 */

		protected override Status InternalTick()
		{
			Status childStatus = _activeChild.Status;

			if (childStatus == Status.Running)
			{
				return Status.Running;
			}
			if (childStatus == Status.Success)
			{
				return Status.Success;
			}
			/*
			 * If the current child has failed, and it was the last one, return
			 * failure.
			 */
			if (_activeChildIndex == _children.Count - 1)
			{
				return Status.Failure;
			}
			/*
				 * Otherwise, if it was not the last child, spawn the next
				 * child.
				 */
			_activeChildIndex++;
			_activeChild = _children[_activeChildIndex].CreateExecutor(Executor, this);
			_activeChild.AddTaskListener(this);
			_activeChild.Spawn(Context);
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
	 * Just calls {@link #tick()} to make the ExecutionSelector evolve.
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
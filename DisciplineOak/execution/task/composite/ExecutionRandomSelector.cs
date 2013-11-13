/**
 * ExecutionRandomSelector is the ExecutionTask that knows how to run a
 * ModelRandomSelector.
 * 
 
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Core.Events;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.Composite;

namespace DisciplineOak.Execution.Task.Composite
{
	public class ExecutionRandomSelector : ExecutionComposite
	{
		private ExecutionTask _activeChild;
		/**
		 * The currently active child of the selector. This integer is an index over
		 * the elements of {@link #order}.
		 */
		private int _activeChildIndex;

		/**
		 * The list of children of this task.
		 */
		private List<ModelTask> _children;

		/**
		 * List storing a sequence of integers with the order in which the children
		 * of this task must be evaluated. This list is computed when the task is
		 * spawned.
		 */
		private List<int> _order;

		/**
		 * Constructs an ExecutionRandomSelector to run a specific ModelRandomSelector.
		 * 
		 * @param modelTask
		 *            the ModelRandomSelector to run.
		 * @param executor
		 *            the BTExecutor that will manage this ExecutionRandomSelector.
		 * @param parent
		 *            the parent ExecutionTask of this task.
		 */

		public ExecutionRandomSelector(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelRandomSelector))
			{
				throw new ArgumentException("The ModelTask must subclass ModelRandomSelector but it inherits from " + modelTask.GetType().Name);
			}
		}

		/**
		 * Spawns the first task (randomly selected).
		 * 
		 * @see jbt.execution.core.ExecutionTask#internalSpawn()
		 */

		protected override void InternalSpawn()
		{
			_children = ModelTask.Children;
			/*
			 * First we initialize the list with the order in which the list of
			 * children will be evaluated.
			 */
			_order = new List<int>();
			for (int i = 0; i < _children.Count; i++)
			{
				_order.Add(i);
			}

			var rnd = new Random();
			_order = _order.OrderBy(i => rnd.Next()).ToList();

			/*
			 * Then we spawn the first child.
			 */
			_activeChildIndex = 0;
			_activeChild = _children[_order[_activeChildIndex]].CreateExecutor(Executor, this);
			_activeChild.AddTaskListener(this);
			_activeChild.Spawn(Context);
		}

		/**
		 * Just terminates the currently active child.
		 * 
		 * @see jbt.execution.core.ExecutionTask#internalTerminate()
		 */

		protected override void InternalTerminate()
		{
			_activeChild.Terminate();
		}

		/**
		 * Checks the status of the currently active child. If it is running,
		 * {@link Status#RUNNING} is returned. If it has successfully finished, it
		 * returns {@link Status#SUCCESS}. If it has failed, it tries to spawn the
		 * next child (returning {@link Status#RUNNING}). If it was the last child,
		 * returns {@link Status#FAILURE}.
		 * 
		 * @see jbt.execution.core.ExecutionTask#internalTick()
		 */

		protected override Status InternalTick()
		{
			var childStatus = _activeChild.Status;

			if (childStatus == Status.Running)
			{
				return Status.Running;
			}
			if (childStatus == Status.Success)
			{
				return Status.Success;
			}

			/* If it was the last child of the list, return failure. */
			if (_activeChildIndex == _children.Count - 1)
			{
				return Status.Failure;
			}

			_activeChildIndex++;
			_activeChild = _children[_order[_activeChildIndex]].CreateExecutor(Executor, this);
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
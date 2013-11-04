﻿/**
 * ExecutionRandomSequence is the ExecutionTask that knows how to run a
 * ModelRandomSequence.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Core.@event;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.composite;

namespace DisciplineOak.Execution.Task.Composite
{
	public class ExecutionRandomSequence : ExecutionComposite
	{
		/**
	 * Currently active child.
	 */
		private ExecutionTask activeChild;
		/**
	 * The currently active child of the sequence. This integer is an index over
	 * the elements of {@link #order}.
	 */
		private int activeChildIndex;
		/**
	 * The list of children of this task.
	 */
		private List<ModelTask> children;
		/**
	 * List storing a sequence of integers with the order in which the children
	 * of this task must be evaluated. This list is computed when the task is
	 * spawned.
	 */
		private List<int> order;

		/**
	 * Constructs an ExecutionRandomSequence to run a specific
	 * ModelRandomSequence.
	 * 
	 * @param modelTask
	 *            the ModelRandomSequence to run.
	 * @param executor
	 *            the BTExecutor that will manage this ExecutionRandomSequence.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

		public ExecutionRandomSequence(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelRandomSequence))
			{
				throw new ArgumentException("The ModelTask must subclass " + typeof (ModelRandomSequence).Name +
				                            " but it inherits from " + modelTask.GetType().Name);
			}
		}

		/**
	 * Spawns the first task (randomly selected).
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalSpawn()
	 */

		protected override void InternalSpawn()
		{
			children = ModelTask.Children;
			/*
		 * First we initialize the list with the order in which the list of
		 * children will be evaluated.
		 */
			order = new List<int>();
			for (int i = 0; i < children.Count; i++)
			{
				order.Add(i);
			}

			var rnd = new Random();
			order = order.OrderBy(i => rnd.Next()).ToList();

			/*
		 * Then we spawn the first child.
		 */
			activeChildIndex = 0;
			activeChild = children[order[activeChildIndex]].CreateExecutor(Executor, this);
			activeChild.AddTaskListener(this);
			activeChild.Spawn(Context);
		}

		/**
	 * Just terminates the currently active child.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTerminate()
	 */

		protected override void InternalTerminate()
		{
			activeChild.Terminate();
		}

		/**
	 * Checks the status of the currently active child. If it is running,
	 * {@link Status#RUNNING} is returned. If it has finished in failure,
	 * {@link Status#FAILURE} is returned. If it has finished successfully, it
	 * tries to spawn the next child (and returns {@link Status#RUNNING}). If it
	 * was the last child of the sequence, returns {@link Status#SUCCESS}.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTick()
	 */

		protected override Status InternalTick()
		{
			Status childStatus = activeChild.GetStatus();

			if (childStatus == Status.Running)
			{
				return Status.Running;
			}
			if (childStatus == Status.Success)
			{
				/* If it was the last child of the sequence, returns success. */
				if (activeChildIndex == children.Count - 1)
				{
					return Status.Success;
				}

				activeChildIndex++;
				activeChild = children[order[activeChildIndex]].CreateExecutor(Executor, this);
				activeChild.AddTaskListener(this);
				activeChild.Spawn(Context);
				return Status.Running;
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
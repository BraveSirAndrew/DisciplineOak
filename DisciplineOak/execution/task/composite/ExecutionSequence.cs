/**
 * ExecutionSequence is the ExecutionTask that knows how to run a ModelSequence
 * task.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using System.Collections.Generic;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Core.@event;
using DisciplineOak.model.Core;
using DisciplineOak.model.Task.Composite;

namespace DisciplineOak.Execution.Task.composite
{
	public class ExecutionSequence : ExecutionTask
	{
		private ExecutionTask _activeChild;
		private int _activeChildIndex;
		/** List of the ModelTask children of the sequence. */
		private List<ModelTask> _children;

		/**
	 * Creates an ExecutionSequence that is able to run a ModelSequence task and
	 * that is managed by a BTExecutor.
	 * 
	 * @param modelTask
	 *            the ModelSequence that this ExecutionSequence is going to run.
	 * @param executor
	 *            the BTExecutor in charge of running this ExecutionSequence.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

		public ExecutionSequence(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelSequence))
			{
				throw new ArgumentException("The ModelTask must subclass " + typeof (ModelSequence).Name + " but it inherits from " +
				                            modelTask.GetType().Name);
			}
		}

		/**
	 * Spawns the first child of the sequence.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalSpawn()
	 */

		protected override void InternalSpawn()
		{
			/*
		 * Spawn the first child of the sequence.
		 */
			_activeChildIndex = 0;
			_children = ModelTask.Children;
			_activeChild = _children[0].CreateExecutor(Executor, this);
			_activeChild.AddTaskListener(this);
			_activeChild.Spawn(Context);
		}

		/**
		 * Terminates the currently active child.
		 * 
		 * @see jbt.execution.core.ExecutionTask#internalTerminate()
		 */

		protected override void InternalTerminate()
		{
			/* Just terminate the active child. */
			_activeChild.Terminate();
		}

		/**
		 * Checks if the currently active child has finished. If it has not
		 * finished, returns {@link Status#SUCCESS}. If it has finished in failure,
		 * returns {@link Status#FAILURE}. If it has finished successfully, it
		 * checks if there is any remaining child. If so, it spawns it. Otherwise,
		 * returns {@link Status#SUCCESS}.
		 * 
		 * @see jbt.execution.core.ExecutionTask#internalTick()
		 */

		protected override Status InternalTick()
		{
			Status childStatus = _activeChild.GetStatus();
			if (childStatus == Status.Running)
			{
				return Status.Running;
			}
			if (childStatus == Status.Failure || childStatus == Status.Terminated)
			{
				return Status.Failure;
			}
			if (_activeChildIndex == _children.Count - 1)
			{
				/*
				 * If this was the last child, return success.
				 */
				return Status.Success;
			}
			/*
				 * If the current child has finished successfully, but it is not
				 * the last one, spawn the next child.
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
	 * Just calls {@link #tick()} to check if the ExecutionSequence can evolve
	 * due to the change in the state of the task that was listening to.
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
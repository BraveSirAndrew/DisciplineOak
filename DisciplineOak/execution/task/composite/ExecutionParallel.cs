﻿/**
 * ExecutionParallel is the ExecutionTask that knows how to run a ModelParallel
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
	public class ExecutionParallel : ExecutionComposite
	{
		/** Policy of the parallel task. */
		/** List of the ExecutionTask children of this task. */
		private List<ExecutionTask> _executionChildren;
		private List<ModelTask> _modelChildren;
		private ParallelPolicy _policy;

		/**
		 * Creates an ExecutionParallel that is able to run a ModelParallel task and
		 * that is managed by a BTExecutor.
		 * 
		 * @param modelTask
		 *            the ModelParallel that this ExecutionParallel is going to run.
		 * @param executor
		 *            the BTExecutor in charge of running this ExecutionParallel.
		 * @param parent
		 *            the parent ExecutionTask of this task.
		 */

		public ExecutionParallel(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelParallel))
			{
				throw new ArgumentException("The ModelTask must subclass " + typeof (ModelParallel).Name + " but it inherits from " +
				                            modelTask.GetType().Name);
			}

			_policy = ((ModelParallel)modelTask).Policy;
			_modelChildren = modelTask.Children;
			Initialize();
		}

		private void Initialize()
		{
			_executionChildren = new List<ExecutionTask>();
		}

		/**
	 * Spawns every single child of the task.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalSpawn()
	 */

		protected override void InternalSpawn()
		{
			if (_policy == ParallelPolicy.SequencePolicy)
			{
				sequencePolicySpawn();
			}
			else
			{
				selectorPolicySpawn();
			}
		}

		/**
	 * Terminates all of its children.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTerminate()
	 */

		protected override void InternalTerminate()
		{
			if (_policy == ParallelPolicy.SequencePolicy)
			{
				sequencePolicyTerminate();
			}
			else
			{
				selectorPolicyTerminate();
			}
		}

		/**
	 * Ticks this ExecutionParallel. This process varies depending on the
	 * policy. that is being followed. See {@link ModelParallel} for more
	 * information.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTick()
	 */

		protected override Status InternalTick()
		{
			if (_policy == ParallelPolicy.SequencePolicy)
			{
				return sequencePolicyTick();
			}
			return selectorPolicyTick();
		}

		/**
	 * Carries out the spawning process when the policy is
	 * {@link ParallelPolicy#SEQUENCE_POLICY}.
	 */

		private void sequencePolicySpawn()
		{
			/* First, create an ExecutionTask for all of the childre. */
			foreach (ModelTask modelChild in _modelChildren)
			{
				_executionChildren.Add(modelChild.CreateExecutor(Executor, this));
			}

			/* Then, spawn them all. */
			foreach (ExecutionTask child in _executionChildren)
			{
				child.AddTaskListener(this);
				child.Spawn(Context);
			}
		}

		/**
	 * Carries out the spawning process when the policy is
	 * {@link ParallelPolicy#SELECTOR_POLICY}.
	 */

		private void selectorPolicySpawn()
		{
			sequencePolicySpawn();
		}

		/**
	 * Carries out the termination process when the policy is
	 * {@link ParallelPolicy#SEQUENCE_POLICY}.
	 */

		private void sequencePolicyTerminate()
		{
			/* Just terminate all of its children. */
			foreach (ExecutionTask child in _executionChildren)
			{
				child.Terminate();
			}
		}

		/**
	 * Carries out the termination process when the policy is
	 * {@link ParallelPolicy#SELECTOR_POLICY}.
	 */

		private void selectorPolicyTerminate()
		{
			sequencePolicyTerminate();
		}

		/**
	 * Carries out the ticking process when the policy is
	 * {@link ParallelPolicy#SEQUENCE_POLICY}.
	 * 
	 * @return the task status after the tick.
	 */

		private Status sequencePolicyTick()
		{
			/*
		 * If one child has failed, then return Status.FAILURE. Otherwise, if
		 * there is at least one child still running, return Status.RUNNING.
		 * Otherwise, return Status.SUCCESS.
		 */
			bool oneRunning = false;

			foreach (ExecutionTask child in _executionChildren)
			{
				Status currentStatus = child.Status;
				if (currentStatus == Status.Running)
				{
					oneRunning = true;
				}
				else if (currentStatus == Status.Failure || currentStatus == Status.Terminated)
				{
					sequencePolicyTerminate();
					return Status.Failure;
				}
			}

			if (!oneRunning)
			{
				return Status.Success;
			}
			return Status.Running;
		}

		/**
	 * Carries out the ticking process when the policy is
	 * {@link ParallelPolicy#SELECTOR_POLICY}.
	 * 
	 * @return the task status after the tick.
	 */

		private Status selectorPolicyTick()
		{
			/*
		 * If one child has succeeded, then return Status.SUCCESS. Otherwise, if
		 * there is at least one child still running, return Status.RUNNING.
		 * Otherwise, return Status.FAILURE.
		 */
			bool oneRunning = false;

			foreach (ExecutionTask child in _executionChildren)
			{
				Status currentStatus = child.Status;
				if (currentStatus == Status.Success)
				{
					selectorPolicyTerminate();
					return Status.Success;
				}
				if (currentStatus == Status.Running)
				{
					oneRunning = true;
				}
			}

			if (!oneRunning)
			{
				return Status.Failure;
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
	 * Just calls {@link #tick()} to make the ExecutionParallel task evolve
	 * according to the status of its children.
	 * 
	 * @see jbt.execution.core.ExecutionTask#statusChanged(jbt.execution.core.event.TaskEvent)
	 */

		public override void StatusChanged(TaskEvent e)
		{
			/*
		 * TODO: the TaskEvent could be used to improve the efficiency of this
		 * method, since we only have to analyse the status of the task that
		 * fired the event, not the status of all the tasks (which is what
		 * tick() does).
		 */
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
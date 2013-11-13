/**
 * ExecutionStaticPriorityList is the ExecutionTask that knows how to run a
 * ModelStaticPriorityList.
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
	public class ExecutionStaticPriorityList : ExecutionComposite
	{
		private ExecutionTask _activeChild;
		private int _activeChildIndex;
		private List<ModelTask> _children;
		/**
	 * List containing the IBTExecutors in charge of running the guards. The
	 * i-th element of this list manages the guard of the i-th child (
	 * {@link #children}). Note that if a guard is null, its corresponding
	 * IBTExecutor is also null.
	 */
		private List<BTExecutor> _guardsExecutors;
		/**
	 * This List contains the current evaluation status of all the guards. If a
	 * guard is null, its corresponding status is {@link Status#SUCCESS} (null
	 * guards are evaluated to true).
	 */
		private List<Status> guardsResults;
		private bool spawnFailed;
		/** Flag that tells if there is a spawned child. */
		private bool stillNotSpawned;

		/**
	 * Creates an ExecutionStaticPriorityList that is able to run a
	 * ModelStaticPriorityList task and that is managed by a BTExecutor.
	 * 
	 * @param modelTask
	 *            the ModelStaticPriorityList that this
	 *            ExecutionStaticPriorityList is going to run.
	 * @param executor
	 *            the BTExecutor in charge of running this
	 *            ExecutionStaticPriorityList.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

		public ExecutionStaticPriorityList(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelStaticPriorityList))
			{
				throw new ArgumentException("The ModelTask must subclass ModelStaticPriorityList but it inherits from " +
				                            modelTask.GetType().Name);
			}
		}

		/**
	 * Spawns the first child with active guard. If there is no active guard,
	 * the spawning process is considered to have failed, so
	 * {@link #internalTick()} will return {@link Status#FAILURE}. If some
	 * guards are still running the spawning process is not considered to have
	 * started yet.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalSpawn()
	 */

		protected override void InternalSpawn()
		{
			_children = ModelTask.Children;

			/* Initialize guard executors. */
			_guardsExecutors = new List<BTExecutor>();
			guardsResults = new List<Status>();
			foreach (ModelTask child in _children)
			{
				if (child.Guard != null)
				{
					_guardsExecutors.Add(new BTExecutor(child.Guard, Context));
					guardsResults.Add(Status.Running);
				}
				else
				{
					_guardsExecutors.Add(null);
					guardsResults.Add(Status.Success);
				}
			}

			/* Evaluate guards. */
			resetGuardsEvaluation();
			Tuple<Status, int> activeGuard = evaluateGuards();

			/*
		 * Flag that tells if the static priority list must be inserted into the
		 * list of tickable nodes.
		 */
			bool insertIntoTickableNodesList = false;

			/*
		 * If all the guards have failed, the spawning process has also failed.
		 * In such a case, the task must be inserted into the list of tickable
		 * nodes.
		 */
			if (activeGuard.Item1 == Status.Failure)
			{
				spawnFailed = true;
				insertIntoTickableNodesList = true;
			}
			else if (activeGuard.Item1 == Status.Running)
			{
				/*
			 * If not all the guards have been evaluated yet, the spawning
			 * process is not considered to have started. In such a case, the
			 * task must be inserted into the list of tickable nodes.
			 */
				stillNotSpawned = true;
				insertIntoTickableNodesList = true;
			}
			else
			{
				/*
			 * If all the guards have been evaluated and one succeeded, spawn
			 * the corresponding child.
			 */
				spawnFailed = false;
				stillNotSpawned = false;
				_activeChildIndex = activeGuard.Item2;
				_activeChild = _children[_activeChildIndex].CreateExecutor(
					Executor, this);
				_activeChild.AddTaskListener(this);
				_activeChild.Spawn(Context);
			}

			/* Insert into the list of tickable nodes if required. */
			if (insertIntoTickableNodesList)
			{
				Executor.RequestInsertionIntoList(BTExecutor.BTExecutorList.Tickable, this);
			}
		}

		/**
	 * If the spawning process has not finished yet (because there are some
	 * guards running), then this method keeps evaluating the guards, and
	 * returns {@link Status#RUNNING}. Whenever there is an active child
	 * (because the spawning process has finished), its status is returned.
	 * <p>
	 * If the spawning process failed, this method just returns
	 * {@link Status#FAILURE}.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTick()
	 */

		protected override Status InternalTick()
		{
			/* If the spawning process failed, return failure. */
			if (spawnFailed)
			{
				return Status.Failure;
			}

			/*
		 * If no child has been spawned yet (not all the guards had completed in
		 * the internalSpawn() method)...
		 */
			if (stillNotSpawned)
			{
				/* Evaluate guards. */
				Tuple<Status, int> activeGuard = evaluateGuards();

				/* If all the guards have failed, return failure. */
				if (activeGuard.Item1 == Status.Failure)
				{
					return Status.Failure;
				}
				if (activeGuard.Item1 == Status.Running)
				{
					/*
				 * If not all the guards have finished, do no nothing (return
				 * RUNNING).
				 */
				}
				else
				{
					/*
				 * If all the guards have been evaluated and one succeeded,
				 * spawn the child. In this case, the static priority list
				 * must be removed from the list of tickable nodes.
				 */
					spawnFailed = false;
					stillNotSpawned = false;
					_activeChildIndex = activeGuard.Item2;
					_activeChild = _children[_activeChildIndex].CreateExecutor(
						Executor, this);
					_activeChild.AddTaskListener(this);
					_activeChild.Spawn(Context);

					Executor.RequestRemovalFromList(BTExecutor.BTExecutorList.Tickable, this);
				}

				return Status.Running;
			}

			/* If this point has been reached, there must be an active child. */
			return _activeChild.GetStatus();
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
	 * @see jbt.execution.core.ExecutionTask#restoreState(jbt.execution.core.ITaskState)
	 */

		protected override void RestoreState(ITaskState state)
		{
		}

		/**
	 * Just ticks this task.
	 * 
	 * @see jbt.execution.core.ExecutionTask#statusChanged(jbt.execution.core.event.TaskEvent)
	 */

		public override void StatusChanged(TaskEvent e)
		{
			Tick();
		}

		/**
	 * Just terminates the currently active child (if there is one).
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTerminate()
	 */

		protected override void InternalTerminate()
		{
			/*
		 * This null check is necessary. Keep in mind that the static priority
		 * list may not have an active child, since the spawning process may
		 * have failed or not started yet. In such a case, if it is terminated,
		 * "this.activeChild" will be null.
		 */
			if (_activeChild != null)
			{
				_activeChild.Terminate();
			}

			/* Terminate the guards executors. */
			foreach (BTExecutor guardExecutor in _guardsExecutors)
			{
				if (guardExecutor != null)
				{
					guardExecutor.Terminate();
				}
			}
		}

		/**
	 * Resets the evaluation of all the guards. This method leaves all the guard
	 * executors ({@link #guardsExecutors}) ready to start again the evaluation
	 * of the guards. It internally terminates the IBTExecutor of each guard,
	 * creates a new one, and then ticks it.
	 */

		private void resetGuardsEvaluation()
		{
			for (int i = 0; i < _guardsExecutors.Count; i++)
			{
				BTExecutor guardExecutor = _guardsExecutors[i];

				if (guardExecutor != null)
				{
					guardExecutor.Terminate();
					guardsResults[i] = Status.Running;
					var newExecutor = new BTExecutor(guardExecutor.GetBehaviourTree(),
						Context);
					newExecutor.CopyTasksStates(guardExecutor);
					newExecutor.Tick();
					_guardsExecutors[i] = newExecutor;
				}
			}
		}

		/**
	 * Evaluate all the guards that have not finished yet, that is, those whose
	 * result in {@link #guardsResults} is {@link Status#RUNNING}, by ticking
	 * them.
	 * <p>
	 * If all the guards have finished in failure, this method returns a Tuple
	 * whose first element is {@link Status#FAILURE}. If there is at least one
	 * guard still being evaluated, the first element of the Tuple contains
	 * {@link Status#RUNNING}. If all the guards have been evaluated and at
	 * least one has succeeded, the first element of the Tuple is
	 * {@link Status#SUCCESS}, and the second one is the index, over the list of
	 * guards ({@link #guardsExecutors}) , of the first guard (that with the
	 * highest priority) that has succeeded.
	 * 
	 */

		private Tuple<Status, int> evaluateGuards()
		{
			bool oneRunning = false;

			/* First, evaluate all the guards that have not finished yet. */
			for (int i = 0; i < _guardsExecutors.Count; i++)
			{
				IBTExecutor guardExecutor = _guardsExecutors[i];
				if (guardExecutor != null)
				{
					if (guardsResults[i] == Status.Running)
					{
						guardExecutor.Tick();
						guardsResults[i] = guardExecutor.GetStatus();
						if (guardsResults[i] == Status.Running)
						{
							oneRunning = true;
						}
					}
				}
			}

			/* If there is at least one still running... */
			if (oneRunning)
			{
				return new Tuple<Status, int>(Status.Running, -1);
			}

			/* If all of them have finished we check which one succeeded first. */
			for (int i = 0; i < guardsResults.Count; i++)
			{
				if (guardsResults[i] == Status.Success)
				{
					return new Tuple<Status, int>(Status.Success, i);
				}
			}

			/* Otherwise, the evaluation has failed. */
			return new Tuple<Status, int>(Status.Failure, -1);
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
/**
 * ExecutionDynamicPriorityList is the ExecutionTask that knows how to run a
 * ModelDynamicPriorityList.
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
	public class ExecutionDynamicPriorityList : ExecutionComposite
	{
		/** List of the children (ModelTask) of this task. */
		/** Currently active child. */
		private const int NumTicksLongTick = 20;
		private ExecutionTask _activeChild;
		private int _activeChildIndex;
		private List<ModelTask> _children;

		/**
	 * List containing the IBTExecutors in charge of running the guards. The i-th element of this
	 * list manages the guard of the i-th child ( {@link #children}). Note that if a guard is null,
	 * its corresponding IBTExecutor is also null.
	 */
		private List<BTExecutor> _guardsExecutors;
		/**
	 * This List contains the current evaluation status of all the guards. If a guard is null, its
	 * corresponding status is {@link Status#SUCCESS} (null guards are evaluated to true).
	 */
		private List<Status> _guardsResults;
		/**
	 * Index of the current most relevant guard. All the guards before it have finished in failure.
	 * This represents the guard such that, if its status changes to success, then it would be the
	 * one selected by the dynamic priority list.
	 */
		private int _indexMostRelevantGuard;
		private bool _spawnFailed;
		/** Flag that tells if there is a spawned child. */
		private bool _stillNotSpawned;

		/**
	 * Creates an ExecutionDynamicPriorityList that is able to run a ModelDynamicPriorityList task
	 * and that is managed by a BTExecutor.
	 * 
	 * @param modelTask
	 *            the ModelDynamicPriorityList that this ExecutionDynamicPriorityList is going to
	 *            run.
	 * @param executor
	 *            the BTExecutor in charge of running this ExecutionDynamicPriorityList.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

		public ExecutionDynamicPriorityList(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelDynamicPriorityList))
			{
				throw new ArgumentException("The ModelTask must subclass " + typeof (ModelDynamicPriorityList).Name +
				                            " but it inherits from " + modelTask.GetType().Name);
			}
		}

		/**
		 * Spawns the first child with active guard. It also requests to be inserted into the list of
		 * tickable nodes of the BTExecutor, since this task has to check its children's guards all the
		 * time. If there is no active guard, the spawning process is considered to have failed, so
		 * {@link #internalTick()} will return {@link Status#FAILURE}. If some guards are still running
		 * the spawning process is not considered to have started yet.
		 * 
		 * @see jbt.execution.core.ExecutionTask#internalSpawn()
		 */
		protected override void InternalSpawn()
		{
			/*
			 * The dynamic priority list has to be inserted into the list of tickable nodes because it
			 * has to check its children's guards all the time.
			 */
			Executor.RequestInsertionIntoList(BTExecutor.BTExecutorList.Tickable, this);

			_children = ModelTask.Children;

			/* Initialize guard executors. */
			_guardsExecutors = new List<BTExecutor>();
			_guardsResults = new List<Status>();

			foreach (var child in _children)
			{
				if (child.Guard != null)
				{
					_guardsExecutors.Add(new BTExecutor(child.Guard, Context));
					_guardsResults.Add(Status.Running);
				}
				else
				{
					_guardsExecutors.Add(null);
					_guardsResults.Add(Status.Success);
				}
			}

			/* Evaluate guards. */
			resetGuardsEvaluation();
			var activeGuard = EvaluateGuards();

			/* If all guards have failed, the spawning process has also failed. */
			if (activeGuard.Item1 == Status.Failure)
			{
				_spawnFailed = true;
			}
			else if (activeGuard.Item1 == Status.Running)
			{
				/*
				 * If not all the guards have been evaluated yet, the spawning process is not considered
				 * to have started.
				 */
				_stillNotSpawned = true;
			}
			else
			{
				
				// If all the guards have been evaluated and one succeeded, spawn the corresponding
				//  child.
				 
				_spawnFailed = false;
				_stillNotSpawned = false;
				_activeChildIndex = activeGuard.Item2;
				_activeChild = _children[_activeChildIndex].CreateExecutor(Executor, this);
				_activeChild.AddTaskListener(this);
				_activeChild.Spawn(Context);

				/* Reset the guards evaluators. */
				resetGuardsEvaluation();
			}
		}

		/**
		 * Just terminates the currently active child (if there is one).
		 * 
		 * @see jbt.execution.core.ExecutionTask#internalTerminate()
		 */

		protected override void InternalTerminate()
		{
			/*
		 * This null check is necessary. Keep in mind that the dynamic priority list may not have an
		 * active child, since the spawning process may have failed or not started yet. In such a
		 * case, if it is terminated, "this.activeChild" will be null.
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
	 * Checks if there is an active guard with a priority higher than that of the active child. If
	 * there is such a task, it terminates the active child and spawns the child of the guard with
	 * higher priority, and {@link Status#RUNNING} is returned. If there is no such task, then the
	 * status of the active child is returned.
	 * <p>
	 * If the spawning process failed, this method just returns {@link Status#FAILURE}. If the
	 * spawning process has not finished yet, this method keeps evaluating the guards, and returns
	 * {@link Status#RUNNING}.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTick()
	 */

		protected override Status InternalTick()
		{
			/* If the spawning process failed, return failure. */
			if (_spawnFailed)
			{
				return Status.Failure;
			}

			/* Evaluate guards. */
			Tuple<Status, int> activeGuard = EvaluateGuards();

			/*
		 * If no child has been spawned yet (not all the guards had completed yet in the
		 * internalSpawn() method)...
		 */
			if (_stillNotSpawned)
			{
				/* If all the guards have failed, return failure. */
				if (activeGuard.Item1 == Status.Failure)
				{
					return Status.Failure;
				}
				if (activeGuard.Item1 == Status.Running)
				{
					/*
				 * If not all the guards have finished, do no nothing (return RUNNING).
				 */
				}
				else
				{
					/*
				 * If all the guards have been evaluated and one succeeded, spawn the child.
				 */
					_spawnFailed = false;
					_stillNotSpawned = false;
					_activeChildIndex = activeGuard.Item2;
					_activeChild = _children[_activeChildIndex].CreateExecutor(Executor, this);
					_activeChild.AddTaskListener(this);
					_activeChild.Spawn(Context);

					/* Reset the guards evaluators. */
					resetGuardsEvaluation();
				}

				return Status.Running;
			}

			/* If this point has been reached, there must be an active child. */
			if (activeGuard.Item1 == Status.Failure)
			{
				/* If all the guards have failed, return failure. */
				return Status.Failure;
			}
			if (activeGuard.Item1 == Status.Running)
			{
				/*
			 * If the guards are being evaluated, return the status of the active child.
			 */
				return _activeChild.GetStatus();
			}
			if (activeGuard.Item2 != _activeChildIndex)
			{
				/*
				 * If the child with the highest priority guard has changed, terminate the currently
				 * active child.
				 */
				_activeChild.Terminate();
				_activeChildIndex = activeGuard.Item2;

				/*
				 * Spawn the new child.
				 */
				_activeChild = _children[_activeChildIndex].CreateExecutor(Executor, this);
				_activeChild.AddTaskListener(this);
				_activeChild.Spawn(Context);

				resetGuardsEvaluation();
				return Status.Running;
			}
			/*
				 * If the child with the highest priority guard has not changed, return the status
				 * of the active child.
				 */
			resetGuardsEvaluation();
			return _activeChild.GetStatus();
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
	 * Resets the evaluation of all the guards. This method leaves all the guard executors (
	 * {@link #guardsExecutors}) ready to start again the evaluation of the guards. It internally
	 * terminates the IBTExecutor of each guard, creates a new one, and then ticks it.
	 */

		private void resetGuardsEvaluation()
		{
			for (int i = 0; i < _guardsExecutors.Count; i++)
			{
				BTExecutor guardExecutor = _guardsExecutors[i];

				if (guardExecutor != null)
				{
					guardExecutor.Terminate();
					_guardsResults[i] = Status.Running;
					var newExecutor = new BTExecutor(guardExecutor.GetBehaviourTree(), Context);
					newExecutor.CopyTasksStates(guardExecutor);
					newExecutor.Tick();
					_guardsExecutors[i] = newExecutor;
				}
			}

			_indexMostRelevantGuard = 0;
		}

		/**
	 * Evaluate all the guards that have not finished yet, that is, those whose result in
	 * {@link #guardsResults} is {@link Status#RUNNING}, by ticking them.
	 * <p>
	 * If all the guards have finished in failure, this method returns a Tuple whose first element is
	 * {@link Status#FAILURE}. If guards' evaluation has not completed yet, the first element of the
	 * Tuple contains {@link Status#RUNNING}. If all the guards have been evaluated and at least one
	 * has succeeded, the first element of the Tuple is {@link Status#SUCCESS}, and the second one is
	 * the index, over the list of guards ({@link #guardsExecutors}) , of the first guard (that with
	 * the highest priority) that has succeeded.
	 * 
	 */

		private Tuple<Status, int> EvaluateGuards()
		{
			/*
		 * Tick all the guards that are still running. If one changes its status to SUCCESS and it
		 * matches the guard associated to "indexMostRelevantGuard", then the guards' evaluation is
		 * over and that is the selected guard.
		 */
			for (int i = 0; i < _guardsExecutors.Count; i++)
			{
				IBTExecutor guardExecutor = _guardsExecutors[i];

				if (guardExecutor != null)
				{
					if (_guardsResults[i] == Status.Running)
					{
						longTick(guardExecutor);

						_guardsResults[i] = guardExecutor.GetStatus();

						if (guardExecutor.GetStatus() != Status.Running)
						{
							/*
						 * If the guard has finished, we check if it matches the
						 * "most relevant guard".
						 */
							if (i == _indexMostRelevantGuard)
							{
								if (guardExecutor.GetStatus() == Status.Success)
								{
									return new Tuple<Status, int>(Status.Success, i);
								}
								/*
								 * If the guard failed, we have to find the next
								 * "most relevant guard" and update "indexMostRelevantGuard"
								 * accordingly. For that we check the status of the following
								 * guards. If we find a successful guard before any running guard,
								 * then the guards' evaluation is over, and that is the selected
								 * guard. If we find a running guard before, then that's the new
								 * "most relevant guard". Otherwise, the evaluation has failed, and
								 * there is no successful guard.
								 */
								bool oneRunning = false;

								for (int k = _indexMostRelevantGuard + 1; k < _guardsExecutors.Count; k++)
								{
									if (_guardsExecutors[k] != null)
									{
										Status currentResult = _guardsExecutors[k].GetStatus();
										if (currentResult == Status.Running)
										{
											_indexMostRelevantGuard = k;
											oneRunning = true;
											break;
										}
										if (currentResult == Status.Success)
										{
											return new Tuple<Status, int>(Status.Success, k);
										}
									}
									else
									{
										return new Tuple<Status, int>(Status.Success, k);
									}
								}

								if (!oneRunning)
								{
									return new Tuple<Status, int>(Status.Failure, -1);
								}
							}
						}
					}
				}
				else
				{
					/* Remember, null guard means successful evaluation. */
					if (i == _indexMostRelevantGuard)
					{
						return new Tuple<Status, int>(Status.Success, i);
					}
				}
			}

			return new Tuple<Status, int>(Status.Running, -1);
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
	 * This method ticks <code>executor</code> {@value #NUM_TICKS_LONG_TICK} times. If the executor
	 * finishes earlier, it is not ticked anymore, and the ticking process stops.
	 * 
	 * @param executor
	 *            the IBTExecutor that is ticked.
	 */

		private void longTick(IBTExecutor executor)
		{
			if (executor.GetStatus() == Status.Running || executor.GetStatus() == Status.Uninitialized)
			{
				int counter = 0;
				do
				{
					executor.Tick();
					counter++;
				} while (executor.GetStatus() == Status.Running && counter < NumTicksLongTick);
			}
		}

		/** Number of ticks performed in each long tick ({@link #longTick(IBTExecutor)}). */
	}
}
/**
 * BTExecutor is the implementation of the IBTExecutor interface.
 * <p>
 * BTs are conceptually modeled by a hierarchy of interconnected ModelTask objects. A BT is
 * represented by the root of the tree, which is a single ModelTask object.
 * <p>
 * Therefore, in order to run a BT, a BTExecutor only needs the root task of the tree (a ModelTask
 * object), and the context (IContext) that will be used by the tree. Keep in mind that a BT is
 * executed within a context that contains information about the game that leaf tasks (actions and
 * conditions) and guards (ModelTask) may need in order to run.
 * <p>
 * The internal implementation of the BTExecutor is based on using, for each ModelTask of the BT, an
 * ExecutionTask obtained by calling the {@link ModelTask#createExecutor(BTExecutor, ExecutionTask)}
 * method. The most important feature of the BTExecutor, however, is that it uses a list of
 * <i>tickable</i> nodes. When {@link #tick()} is called, not all the nodes of the tree are ticked,
 * but only those that are currently relevant to the execution of the tree. By doing so, running the
 * tree is a much more efficient process, since only the nodes that can make the tree evolve receive
 * CPU time.
 * <p>
 * The BTExecutor is also in charge of storing the permanent state of tasks (see {@link ITaskState}
 * and {@link ExecutionTask#storeState()}). For each BT there is only one BTExecutor that actually
 * runs it. Therefore, the BTExecutor can be used as the repository for storing the state of the
 * tasks of the tree.
 * 
 * @see ModelTask
 * @see ExecutionTask
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using System.Collections.Generic;
using DisciplineOak.Execution.Context;
using DisciplineOak.Execution.Task.Decorator;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.decorator;

namespace DisciplineOak.Execution.Core
{
	public class BTExecutor : IBTExecutor
	{
		/** The root task of the BT this executor is running. */
		/** The context that will be passed to the root task. */

		public enum BTExecutorList
		{
			/** Enum for the list of open nodes. */
			Open, /** Enum for the list of tickable nodes. */
			Tickable
		};

		private readonly IContext _context;
		/**
	 * bool telling whether this BTExecutor has been ticked ( {@link #tick()} ) before.
	 */
		/**
	 * List of the tasks that must be inserted into the list of open nodes.
	 */
		private readonly List<ExecutionTask> _currentOpenInsertions;
		/**
	 * List of the tasks that must be removed from the list of open nodes.
	 */
		private readonly List<ExecutionTask> _currentOpenRemovals;
		private readonly List<ExecutionTask> _currentTickableInsertions;
		/**
	 * List of the tasks that must be removed from the list of tickable nodes.
	 */
		/*
	 * TODO: improve the way requests for insertions and removals are handled. Currently simple
	 * Lists are used to manage them, but in case there were many requests, this process would not
	 * be very efficient.
	 */
		private readonly List<ExecutionTask> _currentTickableRemovals;
		/**
	 * List of all the ExecutionInterrupter currently active in the behaviour tree. They are indexed
	 * by their ModelInterrupter in the conceptual tree.
	 * <p>
	 * This list is used by ExecutionPerformInterruption, which must have a way of knowing what
	 * ExecutionInterrupter it is interrupting.
	 */
		private readonly Dictionary<ModelInterrupter, ExecutionInterrupter> _interrupters;
		private readonly ModelTask _modelBT;
		private readonly List<ExecutionTask> _openTasks;
		private readonly List<ExecutionTask> _tickableTasks;
		private ExecutionTask _executionBT;
		private bool firstTimeTicked = true;
		/**
	 * States of the tasks of the tree that is being run by this BTExecutor. States are indexed by
	 * Position. These are the positions of the ExecutionTask in the execution tree. These positions
	 * are unique (they do not necessarily match the position of the corresponding ModelTask), so
	 * each node in the execution tree can be unambiguously referenced by such position. Note that
	 * this Map does not store the states of the nodes of the guards of the tree that is being run.
	 */
		private Dictionary<Position, ITaskState> _tasksStates;

		/**
		 * Creates a BTExecutor that handles the execution of a behaviour tree. The behaviour tree is
		 * represented by a ModelTask (the root of the tree).
		 * <p>
		 * A context for the tree must be provided. This context is passed to the root of the tree, and,
		 * in general, it will be shared by all the nodes in the tree (it will be passed down the
		 * hierarchy of the tree). Note however that, depending on the semantics of the tree itself,
		 * some nodes may not use this context.
		 * 
		 * @param modelBT
		 *            the root of the behaviour tree to run.
		 * @param context
		 *            the initial context for the tree.
		 */
		public BTExecutor(ModelTask modelBT, IContext context)
		{
			if (modelBT == null)
			{
				throw new ArgumentException("The input ModelTask cannot be null");
			}

			if (context == null)
			{
				throw new ArgumentException("The input IContext cannot be null");
			}

			_modelBT = modelBT;
			_modelBT.ComputePositions();
			_context = context;
			_tickableTasks = new List<ExecutionTask>();
			_openTasks = new List<ExecutionTask>();
			_currentOpenInsertions = new List<ExecutionTask>();
			_currentOpenRemovals = new List<ExecutionTask>();
			_currentTickableInsertions = new List<ExecutionTask>();
			_currentTickableRemovals = new List<ExecutionTask>();
			_interrupters = new Dictionary<ModelInterrupter, ExecutionInterrupter>();
			_tasksStates = new Dictionary<Position, ITaskState>();
		}

		/**
		 * Creates a BTExecutor that handles the execution of a behaviour tree. The behaviour tree is
		 * represented by a ModelTask (the root of the tree).
		 * <p>
		 * A new empty context for the tree is created. This context is passed to the root of the tree,
		 * and, in general, it will be shared by all the nodes in the tree (it will be passed down the
		 * hierarchy of the tree). Note however that, depending on the semantics of the tree itself,
		 * some nodes may not use the context context.
		 * 
		 * @param modelBT
		 *            the root of the behaviour tree to run.
		 */

		public BTExecutor(ModelTask modelBT)
		{
			if (modelBT == null)
			{
				throw new ArgumentException("The input ModelTask cannot be null");
			}

			this._modelBT = modelBT;
			this._modelBT.ComputePositions();
			_context = new BasicContext();
			_tickableTasks = new List<ExecutionTask>();
			_openTasks = new List<ExecutionTask>();
			_currentOpenInsertions = new List<ExecutionTask>();
			_currentOpenRemovals = new List<ExecutionTask>();
			_currentTickableInsertions = new List<ExecutionTask>();
			_currentTickableRemovals = new List<ExecutionTask>();
			_interrupters = new Dictionary<ModelInterrupter, ExecutionInterrupter>();
			_tasksStates = new Dictionary<Position, ITaskState>();
		}

		public void Tick()
		{
			/*
			 * The ticking algorithm works as follows:
			 * 
			 * If it is the very first time that this method is called, an ExecutionTask is created from
			 * the root ModelTask (that is, the root of the behaviour tree that this BTExecutor is going
			 * to run). Then, that task is spawned.
			 * 
			 * From then on, tick() will just call tick() on all the ExecutionTasks in the list of
			 * tickable tasks.
			 * 
			 * It is important to note that insertions and removals from the list of tickable and open
			 * tasks are processed at the very beginning and at the very end of this method, but not
			 * while it is ticking the current list of tickable tasks.
			 */
			var currentStatus = GetStatus();

			/* We only tick if the tree has not finished yet or if it has not started running. */
			if (currentStatus == Status.Running || currentStatus == Status.Uninitialized)
			{
				ProcessInsertionsAndRemovals();

				if (firstTimeTicked)
				{
					_executionBT = _modelBT.CreateExecutor(this, null);
					_executionBT.Spawn(_context);
					firstTimeTicked = false;
				}
				else
				{
					foreach (var executionTask in _tickableTasks)
					{
						executionTask.Tick();
					}
				}

				ProcessInsertionsAndRemovals();
			}
		}

		public void Terminate()
		{
			if (_executionBT != null)
			{
				_executionBT.Terminate();
			}
		}

		public ModelTask GetBehaviourTree()
		{
			return _modelBT;
		}

		public Status GetStatus()
		{
			if (_executionBT == null)
			{
				return Status.Uninitialized;
			}
			return _executionBT.GetStatus();
		}

		public IContext GetRootContext()
		{
			return _context;
		}

		/**
		 * Returns the ExecutionInterrupter that is currently active and registered in the BTExecutor (
		 * {@link #registerInterrupter(ExecutionInterrupter)}) whose associated ModelInterrupter is
		 * <code>modelInterrupter</code>. Returns null if there is no such an ExecutionInterrupter.
		 * 
		 * @param modelInterrupter
		 *            the ModelInterrupter associated to the ExecutionInterrupter to retrieve.
		 * @return the ExecutionInterrupter whose associated ModelInterrupter is
		 *         <code>modelInterrupter</code>.
		 */

		public ExecutionInterrupter GetExecutionInterrupter(ModelInterrupter modelInterrupter)
		{
			return _interrupters[modelInterrupter];
		}

		/**
	 * Registers an ExecutionInterrupter with this BTExecutor.
	 * 
	 * @param interrupter
	 *            the ExecutionInterrupter to register.
	 */

		public void RegisterInterrupter(ExecutionInterrupter interrupter)
		{
			if (_interrupters.ContainsKey((ModelInterrupter) interrupter.ModelTask))
				return;

			_interrupters.Add((ModelInterrupter) interrupter.ModelTask, interrupter);
		}

		/**
		 * Unregisters an ExecutionInterrupter from this BTExecutor.
		 * 
		 * @param interrupter
		 *            the ExecutionInterrupter to unregister.
		 */

		public void UnregisterInterrupter(ExecutionInterrupter interrupter)
		{
			if (_interrupters.ContainsKey((ModelInterrupter) interrupter.ModelTask) == false)
				return;

			_interrupters.Remove((ModelInterrupter) interrupter.ModelTask);
		}

		/**
		 * Method used to request the BTExecutor to insert an ExecutionTask into one of the list that it
		 * handles. The insertion is not performed right away, but delayed until:
		 * 
		 * <ul>
		 * <li>Either the current game AI cycle (call to {@link #tick()}) finishes. This happens if the
		 * insertion is requested in the middle of an AI cycle, that is, if <code>tick()</code> is still
		 * running.
		 * <li>Or the next AI cycle starts. This happens if the insertion is requested when the
		 * BTExecutor is not ticking the underlying BT. In this case, the next time <code>tick()</code>
		 * is called, the insertion will be processed just before the BT is actually ticked.
		 * </ul>
		 * 
		 * @param listType
		 *            the type of the list that the task will be inserted into.
		 * @param t
		 *            the task that wants to be inserted into the list of type <code>listType</code>.
		 */

		public void RequestInsertionIntoList(BTExecutorList listType, ExecutionTask t)
		{
			if (listType == BTExecutorList.Open)
			{
				if (!_currentOpenInsertions.Contains(t))
				{
					_currentOpenInsertions.Add(t);
				}
			}
			else
			{
				if (!_currentTickableInsertions.Contains(t))
				{
					_currentTickableInsertions.Add(t);
				}
			}
		}

		/**
		 * Method used to request the BTExecutor to remove an ExecutionTask from one of the list that
		 * the BTExecutor handles. The removal is not performed right away, but delayed until:
		 * 
		 * <ul>
		 * <li>Either the current game AI cycle (call to {@link #tick()}) finishes. This happens if the
		 * removal is requested in the middle of an AI cycle, that is, if <code>tick()</code> is still
		 * running.
		 * <li>Or the next AI cycle starts. This happens if the removal is requested when the BTExecutor
		 * is not ticking the underlying BT. In this case, the next time <code>tick()</code> is called,
		 * the removal will be processed just before the BT is actually ticked.
		 * </ul>
		 * 
		 * @param listType
		 *            the type of the list from which the task will be removed.
		 * @param t
		 *            the task that wants to be removed from the list of type <code>listType</code>.
		 */
		public void RequestRemovalFromList(BTExecutorList listType, ExecutionTask t)
		{
			if (listType == BTExecutorList.Open)
			{
				if (!_currentOpenRemovals.Contains(t))
				{
					_currentOpenRemovals.Add(t);
				}
			}
			else
			{
				if (!_currentTickableRemovals.Contains(t))
				{
					_currentTickableRemovals.Add(t);
				}
			}
		}

		/**
		 * Cancels a previous request of insertion into one of the lists that the BTExecutor handles. If
		 * no such insertion request was made, this method does nothing.
		 * 
		 * @param listType
		 *            the list from which the insertion request will be canceled.
		 * @param t
		 *            the task whose insertion will be canceled.
		 */
		public void CancelInsertionRequest(BTExecutorList listType, ExecutionTask t)
		{
			if (listType == BTExecutorList.Open)
			{
				_currentOpenInsertions.Remove(t);
			}
			else
			{
				_currentTickableInsertions.Remove(t);
			}
		}

		/**
		 * Cancels a previous request of removal from one of the lists that the BTExecutor handles. If
		 * no such removal request was made, this method does nothing.
		 * 
		 * @param listType
		 *            the list from which the removal request will be canceled.
		 * @param t
		 *            the task whose removal will be canceled.
		 */
		public void CancelRemovalRequest(BTExecutorList listType, ExecutionTask t)
		{
			if (listType == BTExecutorList.Open)
			{
				_currentOpenRemovals.Remove(t);
			}
			else
			{
				_currentTickableRemovals.Remove(t);
			}
		}

		/**
		 * Method that processes the insertions and removals into and from the lists of tickable and
		 * open nodes that have been previously requested via the <code>requestXXX</code> methods.
		 * <p>
		 * After calling this method, all pending removals and insertions are processed, so no new
		 * insertion and removal will be carried out unless new ones are requested.
		 */
		private void ProcessInsertionsAndRemovals()
		{
			/*
		 * Process insertions and removals.
		 */
			foreach (var currentTickableInsertion in _currentTickableInsertions)
			{
				_tickableTasks.Add(currentTickableInsertion);
			}

			foreach (var currentTickableRemoval in _currentTickableRemovals)
			{
				_tickableTasks.Remove(currentTickableRemoval);
			}

			foreach (var currentOpenInsertion in _currentOpenInsertions)
			{
				_openTasks.Add(currentOpenInsertion);
			}

			foreach (var currentOpenRemoval in _currentOpenRemovals)
			{
				_openTasks.Remove(currentOpenRemoval);
			}

			/*
		 * Clear the lists of tasks to insert and remove.
		 */
			_currentOpenInsertions.Clear();
			_currentOpenRemovals.Clear();
			_currentTickableInsertions.Clear();
			_currentTickableRemovals.Clear();
		}

		/**
	 * 
	 * @see jbt.execution.core.IBTExecutor#getBehaviourTree()
	 */

		/**
	 * Sets the permanent state of a given task. The task is identified by the position it occupies
	 * in the execution behaviour tree, which unambiguously identifies it.
	 * 
	 * @param taskPosition
	 *            the position of the task whose state must be stored.
	 * @param state
	 *            the state of the task, or null if it should be cleared.
	 * @return true if there was a previous state for this task in the BTExecutor, or false
	 *         otherwise.
	 */

		public bool SetTaskState(Position taskPosition, ITaskState state)
		{
			if (state == null)
			{
				return _tasksStates.Remove(taskPosition);
			}

			if (_tasksStates.ContainsKey(taskPosition))
			{
				_tasksStates.Add(taskPosition, state);
				return false;
			}
			_tasksStates[taskPosition] = state;
			return true;
		}

		/**
	 * Returns the permanent state of a task. The task is identified by the position it occupies in
	 * the execution behaviour tree, which unambiguously identifies it.
	 * 
	 * @param taskPosition
	 *            the position of the task whose state must be retrieved.
	 * @return the state of the task, or null if there is no state stored in the BTExecutor for the
	 *         task.
	 */

		public ITaskState GetTaskState(Position taskPosition)
		{
			return _tasksStates[taskPosition];
		}

		/**
	 * Copies the set of all tasks' states stored in <code>executor</code> into this BTExecutor.
	 * <p>
	 * <b>After calling this method, the set of all tasks' states is shared by both BTExecutor
	 * objects (<code>executor</code> and <code>this</code>), so if one modifies it, the other will
	 * notice the change.</b>
	 */

		public void CopyTasksStates(BTExecutor executor)
		{
			_tasksStates = executor._tasksStates;
		}

		/**
	 * Clears the permanent state of a task. The task is identified by the position it occupies in
	 * the execution behaviour tree, which unambiguously identifies it.
	 * 
	 * @param taskPosition
	 *            the position of the task whose state must be cleared.
	 * @return true if the BTExecutor contained the state of the task before calling this method, or
	 *         false otherwise.
	 */

		public bool ClearTaskState(Position taskPosition)
		{
			return _tasksStates.Remove(taskPosition);
		}

		public override string ToString()
		{
			return "[Root: " + _modelBT.GetType().Name + ", Status: " + GetStatus() + "]";
		}
	}
}
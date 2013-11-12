/**
 * ExecutionLimit is the ExecutionTask that knows how to run a ModelLimit.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using System.Collections.Generic;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Core.@event;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.decorator;

namespace DisciplineOak.Execution.Task.Decorator
{
	public class ExecutionLimit : ExecutionDecorator
	{
		/**
		 * Name of the variable that is stored in the context and that represents
		 * the number of times that the decorator has been run so far.
		 */
		private const string StateVariableName = "RunsSoFar";

		/** Maximum number of times that the child task can be executed. */
		private readonly int _maxNumTimes;
		private ExecutionTask _child;

		/**
		 * Number of times that the child task has been run so far. Initially, its
		 * value is restored from the context.
		 */
		private int _numRunsSoFar;

		/**
		 * Creates an ExecutionLimit that knows how to run a ModelLimit.
		 * 
		 * @param modelTask
		 *            the ModelLimit to run.
		 * @param executor
		 *            the BTExecutor that will manage this ExecutionLimit.
		 * @param parent
		 *            the parent ExecutionTask of this task.
		 */
		public ExecutionLimit(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelLimit))
			{
				throw new ArgumentException("The ModelTask must subclass ModelLimitbut it inherits from " + modelTask.GetType().Name);
			}

			_maxNumTimes = ((ModelLimit) ModelTask).getMaxNumTimes();
			_numRunsSoFar = 0;
		}

		/**
		 * Spawns the child task if it has not been run more than the maximum
		 * allowed number of times. Otherwise, it requests to be inserted into the
		 * list of tickable nodes, since the child is not spawned.
		 * 
		 * @see jbt.execution.core.ExecutionTask#internalSpawn()
		 */

		protected override void InternalSpawn()
		{
			if (_numRunsSoFar < _maxNumTimes)
			{
				_numRunsSoFar++;
				_child = ((ModelLimit) ModelTask).getChild().CreateExecutor(Executor, this);
				_child.AddTaskListener(this);
				_child.Spawn(Context);
			}
			else
			{
				Executor.RequestInsertionIntoList(BTExecutor.BTExecutorList.Tickable, this);
			}
		}

		/**
		 * Terminates the child task.
		 * 
		 * @see jbt.execution.core.ExecutionTask#internalTerminate()
		 */

		protected override void InternalTerminate()
		{
			if (_child != null)
			{
				_child.Terminate();
			}
		}

		/**
		 * Returns the status of the child task, or {@link Status#FAILURE} in case
		 * it could not be spawned.
		 * 
		 * @see jbt.execution.core.ExecutionTask#internalTick()
		 */

		protected override Status InternalTick()
		{
			return _child != null ? _child.GetStatus() : Status.Failure;
		}

		/**
		 * Restore from the ITaskState the number of times that the child task of
		 * this decorator has been run so far. It is read from the variable whose
		 * name is {@link #STATE_VARIABLE_NAME}.
		 * 
		 * @see jbt.execution.core.ExecutionTask#restoreState(ITaskState)
		 */

		protected override void RestoreState(ITaskState state)
		{
			try
			{
				_numRunsSoFar = (int) state.GetStateVariable(StateVariableName);
			}
			catch (Exception e)
			{
			}
		}

		/**
		 * Just calls {@link #tick()} to make this task evolve.
		 * 
		 * @see jbt.execution.core.ExecutionTask#statusChanged(jbt.execution.core.event.TaskEvent)
		 */
		public override void StatusChanged(TaskEvent e)
		{
			Tick();
		}

		/**
		 * Returns an ITaskState witht a variable with name
		 * {@link #STATE_VARIABLE_NAME} , and whose value is the number of times
		 * that the child task of this decorator has been run so far.
		 * 
		 * @see jbt.execution.core.ExecutionTask#storeState()
		 */
		protected override ITaskState StoreState()
		{
			var variables = new List<Tuple<string, Object>> {new Tuple<string, Object>(StateVariableName, _numRunsSoFar)};
			return TaskStateFactory.CreateTaskState(variables);
		}

		/**
		 * Returns an ITaskState witht a variable with name
		 * {@link #STATE_VARIABLE_NAME} , and whose value is the number of times
		 * that the child task of this decorator has been run so far.
		 * 
		 * @see jbt.execution.core.ExecutionTask#storeTerminationState()
		 */
		protected override ITaskState StoreTerminationState()
		{
			var variables = new List<Tuple<string, Object>> {new Tuple<string, Object>(StateVariableName, _numRunsSoFar)};
			return TaskStateFactory.CreateTaskState(variables);
		}
	}
}
/**
 * An ExecutionFailure is the ExecutionTask that knows how to run a
 * ModelFailure.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.Leaf;

namespace DisciplineOak.Execution.Task.Leaf
{
	public class ExecutionFailure : ExecutionLeaf
	{
		/**
	 * Constructs an ExecutionFailure that knows how to run a ModelFailure.
	 * 
	 * @param modelTask
	 *            the ModelFailure to run.
	 * @param executor
	 *            the BTExecutor managing this ExecutionFailure.
	 * @param parent
	 *            the parent ExecutionTask.
	 */

		public ExecutionFailure(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelFailure))
			{
				throw new ArgumentException("The ModelTask must subclass ModelFailure but it inherits from " +
				                            modelTask.GetType().Name);
			}
		}

		/**
	 * Does nothing.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalSpawn()
	 */

		protected override void InternalSpawn()
		{
			Executor.RequestInsertionIntoList(BTExecutor.BTExecutorList.Tickable, this);
		}

		/**
	 * Returns {@link Status#FAILURE}.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTick()
	 */

		protected override Status InternalTick()
		{
			return Status.Failure;
		}

		/**
	 * Returns null.
	 * 
	 * @see jbt.execution.core.ExecutionTask#storeState()
	 */

		protected override ITaskState StoreState()
		{
			return null;
		}

		/**
	 * Returns null.
	 * 
	 * @see jbt.execution.core.ExecutionTask#storeTerminationState()
	 */

		protected override ITaskState StoreTerminationState()
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
	 * Does nothing.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTerminate()
	 */

		protected override void InternalTerminate()
		{
		}
	}
}
/**
 * An ExecutionSuccess is the ExecutionTask that knows how to run a
 * ModelSuccess.
 * 
 
 * 
 */

using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.Leaf;

namespace DisciplineOak.Execution.Task.Leaf
{
	public class ExecutionSuccess : ExecutionLeaf
	{
		/**
	 * Constructs an ExecutionSuccess that knows how to run a ModelSuccess.
	 * 
	 * @param modelTask
	 *            the ModelSuccess to run.
	 * @param executor
	 *            the BTExecutor managing this ExecutionSuccess.
	 * @param parent
	 *            the parent ExecutionTask.
	 */

		public ExecutionSuccess(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelSuccess))
			{
				throw new ArgumentException("The ModelTask must subclass ModelSuccess but it inherits from " +
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
	 * Returns {@link Status#SUCCESS}.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTick()
	 */

		protected override Status InternalTick()
		{
			return Status.Success;
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
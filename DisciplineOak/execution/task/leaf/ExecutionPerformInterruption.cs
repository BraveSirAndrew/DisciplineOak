/**
 * ExecutionPerformInterruption is the ExecutionTask that knows how to run a
 * ModelPerformInterrupter.
 * 
 
 * 
 */

using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.Leaf;

namespace DisciplineOak.Execution.Task.Leaf
{
	public class ExecutionPerformInterruption : ExecutionLeaf
	{
		/**
	 * Creates an ExecutionPerformInterruption that is able to run a
	 * ModelPerformInterruption task and that is managed by a BTExecutor.
	 * 
	 * @param modelTask
	 *            the ModelPerformInterruption that this
	 *            ExecutionPerformInterruption is going to run.
	 * @param executor
	 *            the BTExecutor in charge of running this
	 *            ExecutionPerformInterruption.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

		public ExecutionPerformInterruption(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelPerformInterruption))
			{
				throw new ArgumentException("The ModelTask must subclass ModelPerformInterruption but it inherits from " +
				                            modelTask.GetType().Name);
			}
		}

		/**
	 * Calls {@link ExecutionInterrupter#interrupt(Status)} on the
	 * ExecutionInterrupter associated to this ExecutionPerformInterruption.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalSpawn()
	 */

		protected override void InternalSpawn()
		{
			Executor.RequestInsertionIntoList(BTExecutor.BTExecutorList.Tickable, this);
			
			foreach (var modelInterrupter in ((ModelPerformInterruption)ModelTask).Interrupters)
			{
				var interrupter = Executor.GetExecutionInterrupter(modelInterrupter);
				if (interrupter != null)
				{
					interrupter.Interrupt(((ModelPerformInterruption)ModelTask).DesiredResult);
				}
			}
		}

		/**
	 * Does nothing.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTerminate()
	 */

		protected override void InternalTerminate()
		{
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
	 * Does nothing.
	 * 
	 * @see jbt.execution.core.ExecutionTask#restoreState(ITaskState)
	 */

		protected override void RestoreState(ITaskState state)
		{
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
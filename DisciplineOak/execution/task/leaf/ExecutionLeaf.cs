/**
 * Base class for all the ExecutionTask classes that are able to run leaf tasks,
 * that is, classes that inherit from ModelLeaf.
 * 
 */

using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Core.Events;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.Leaf;

namespace DisciplineOak.Execution.Task.Leaf
{
	public abstract class ExecutionLeaf : ExecutionTask
	{
		/**
	 * Constructs an ExecutionLeaf to run a specific ModelLeaf.
	 * 
	 * @param modelTask
	 *            the ModelLeaf to run.
	 * @param executor
	 *            the BTExecutor that will manage this ExecutionLeaf.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

		protected ExecutionLeaf(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelLeaf))
			{
				throw new ArgumentException("The ModelTask must subclass ModelLeaf but it inherits from " + modelTask.GetType().Name);
			}
		}

		/**
	 * Does nothing by default, since a leaf task has no children.
	 * 
	 * @see jbt.execution.core.ExecutionTask#statusChanged(jbt.execution.core.event.TaskEvent)
	 */
		public override void StatusChanged(TaskEvent e)
		{
		}
	}
}
/**
 * ExecutionAction is the base class of all of the class that are able to run
 * actions in the game (that is, subclasses of ModelAction).
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.Leaf.action;

namespace DisciplineOak.Execution.Task.Leaf.Action
{
	public abstract class ExecutionAction : ExecutionLeaf
	{
		/**
	 * Constructs an ExecutionAction that knows how to run a ModelAction.
	 * 
	 * @param modelTask
	 *            the ModelAction to run.
	 * @param executor
	 *            the BTExecutor that will manage this ExecutionAction.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

		protected ExecutionAction(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelAction))
			{
				throw new ArgumentException("The ModelTask must subclass ModelAction but it inherits from " +
				                            modelTask.GetType().Name);
			}
		}
	}
}
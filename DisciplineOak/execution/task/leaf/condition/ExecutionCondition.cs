/**
 * ExecutionCondition is the base class of all of the class that are able to run
 * conditions in the game (that is, subclasses of ModelCondition).
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.model.Core;
using DisciplineOak.model.Task.Leaf.condition;

namespace DisciplineOak.Execution.Task.leaf.condition
{
	public abstract class ExecutionCondition : ExecutionLeaf
	{
		/**
		 * Constructs an ExecutionCondition that knows how to run a ModelCondition.
		 * 
		 * @param modelTask
		 *            the ModelCondition to run.
		 * @param executor
		 *            the BTExecutor that will manage this ExecutionCondition.
		 * @param parent
		 *            the parent ExecutionTask of this task.
		 */

		protected ExecutionCondition(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelCondition))
			{
				throw new ArgumentException("The ModelTask must subclass ModelCondition but it inherits from " + modelTask.GetType().Name);
			}
		}
	}
}
/**
 * ExecutionCondition is the base class of all of the class that are able to run
 * conditions in the game (that is, subclasses of ModelCondition).
 * 
 
 * 
 */

using DisciplineOak.Execution.Core;
using DisciplineOak.Model.Task.Leaf.Condition;

namespace DisciplineOak.Execution.Task.Leaf.Condition
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

		protected ExecutionCondition(ModelCondition modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			
		}
	}
}
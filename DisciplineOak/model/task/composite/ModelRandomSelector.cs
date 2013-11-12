/**
 * A ModelRandomSelector is a task that behaves just like a ModelSelector, but
 * which walk through its children in a random order. Instead of evaluating its
 * children from left to right, this task evaluate them in a random order.
 * 
 
 * 
 */

using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Composite;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Composite
{
	public class ModelRandomSelector : ModelComposite
	{
		/**
	 * Creates a ModelRandomSelector with a guard and several children. The list
	 * of children cannot be empty.
	 * 
	 * @param guard
	 *            the guard of the ModelRandomSelector, which may be null.
	 * @param children
	 *            the list of children, which cannot be empty.
	 */

		public ModelRandomSelector(ModelTask guard, params ModelTask[] children) : base(guard, children)
		{
		}

		/**
	 * Returns an ExecutionRandomSelector that knows how to run this
	 * ModelRandomSelector.
	 * 
	 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
	 *      ExecutionTask)
	 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionRandomSelector(this, executor, parent);
		}
	}
}
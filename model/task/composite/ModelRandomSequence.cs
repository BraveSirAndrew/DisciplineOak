/**
 * A ModelRandomSequence is a task that behaves just like a ModelSequence, but
 * which walk through its children in a random order. Instead of evaluating its
 * children from left to right, this task evaluate them in a random order.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using OhBehave.Execution.Core;
using OhBehave.execution.core;
using OhBehave.Execution.Task.composite;
using OhBehave.Model.Core;

namespace OhBehave.Model.Task.composite
{
	public class ModelRandomSequence : ModelComposite
	{
		/**
	 * Creates a ModelRandomSequence with a guard and several children. The list
	 * of children cannot be empty.
	 * 
	 * @param guard
	 *            the guard of the ModelRandomSequence, which may be null.
	 * @param children
	 *            the list of children, which cannot be empty.
	 */

		public ModelRandomSequence(ModelTask guard, params ModelTask[] children) : base(guard, children)
		{
		}

		/**
	 * Returns an ExecutionRandomSequence that knows how to run this
	 * ModelRandomSequence.
	 * 
	 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
	 *      ExecutionTask)
	 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionRandomSequence(this, executor, parent);
		}
	}
}
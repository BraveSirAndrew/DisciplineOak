/**
 * A ModelSuccess represents a task that always succeeds.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using OhBehave.Execution.Core;
using OhBehave.execution.core;
using OhBehave.Execution.Task.Leaf;
using OhBehave.Model.Core;

namespace OhBehave.Model.Task.Leaf
{
	public class ModelSuccess : ModelLeaf
	{
		/**
		 * Constructor.
		 * 
		 * @param guard
		 *            the guard of the ModelSuccess, which may be null.
		 */
		public ModelSuccess(ModelTask guard)
			: base(guard)
		{
		}

		/**
		 * Returns an {@link ExecutionSuccess} that knows how to run this
		 * ModelSuccess.
		 */
		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionSuccess(this, executor, parent);
		}
	}
}
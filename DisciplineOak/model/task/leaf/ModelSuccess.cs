/**
 * A ModelSuccess represents a task that always succeeds.
 * 
 
 * 
 */

using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Leaf;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Leaf
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
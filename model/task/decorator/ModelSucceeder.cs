/**
 * A ModelSucceeder is a decorator that makes its child succeeds no matter it it actually fails.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using OhBehave.Execution.Core;
using OhBehave.execution.core;
using OhBehave.Execution.Task.decorator;
using OhBehave.Model.Core;

namespace OhBehave.Model.Task.decorator
{
	public class ModelSucceeder : ModelDecorator
	{
		/**
	 * Constructor.
	 * 
	 * @param guard
	 *            the guard of the ModelSucceeder, which may be null.
	 * @param child
	 *            the child task.
	 */

		public ModelSucceeder(ModelTask guard, ModelTask child) : base(guard, child)
		{
		}

		/**
	 * Returns an ExecutionSucceeder that is able to run this ModelSucceeder.
	 * 
	 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
	 *      ExecutionTask)
	 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionSucceeder(this, executor, parent);
		}
	}
}
/**
 * ModelRepeat represents a decorator that runs its child task forever. When its
 * child task finishes, it runs it once more. This decorator always return
 * {@link Status#RUNNING}.
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
	public class ModelRepeat : ModelDecorator
	{
		/**
	 * Constructor.
	 * 
	 * @param guard
	 *            the guard of the ModelRepeat, which may be null.
	 * @param child
	 *            the child that will be run forever.
	 */

		public ModelRepeat(ModelTask guard, ModelTask child) : base(guard, child)
		{
		}

		/**
	 * Returns an ExecutionForever that knows how to run this ModelRepeat.
	 * 
	 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
	 *      ExecutionTask)
	 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionRepeat(this, executor, parent);
		}
	}
}
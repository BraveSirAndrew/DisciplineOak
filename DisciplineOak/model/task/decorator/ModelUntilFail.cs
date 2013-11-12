/**
 * The ModelUntilFail class represents a decorator used to run a task as long as
 * it does not fail.
 * <p>
 * ModelUntilFail just keeps executing its child task as long as it does not
 * fail. When the child task fails, ModelUntilFail returns
 * {@link Status#SUCCESS}. Otherwise it returns {@link Status#RUNNING}.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.decorator;
using DisciplineOak.model.Core;

namespace DisciplineOak.model.Task.Decorator
{
	public class ModelUntilFail : ModelDecorator
	{
		/**
	 * Constructor.
	 * 
	 * @param guard
	 *            the guard of the ModelUntilFail, which may be null.
	 * @param child
	 *            the task that will be run until it fails.
	 */

		public ModelUntilFail(ModelTask guard, ModelTask child) : base(guard, child)
		{
		}

		/**
	 * Returns an ExecutionUntilFail that knows how to run this ModelUntilFail.
	 * 
	 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
	 *      ExecutionTask)
	 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionUntilFail(this, executor, parent);
		}
	}
}
/**
 * A ModelSafeContextManager is a decorator that creates a new context for its
 * child task. The context that it creates is a {@link SafeContext}, and the
 * input context that the SafeContext receives is the context of the
 * ModelSafeContextManager.
 * <p>
 * The spawning and updating of the child task are carried out as usual.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Decorator;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.decorator
{
	public class ModelSafeContextManager : ModelDecorator
	{
		/**
	 * Constructor.
	 * 
	 * @param guard
	 *            the guard of the ModelSafeContextManager, which may be null.
	 * @param child
	 *            the child of the ModelSafeContextManager.
	 */

		public ModelSafeContextManager(ModelTask guard, ModelTask child) : base(guard, child)
		{
		}

		/**
	 * Returns an ExecutionSafeContextManager that knows how to run this
	 * ModelSafeContextManager.
	 * 
	 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
	 *      ExecutionTask)
	 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionSafeContextManager(this, executor, parent);
		}
	}
}
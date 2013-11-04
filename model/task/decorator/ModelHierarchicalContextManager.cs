/**
 * A ModelHierarchicalContextManager is a decorator that creates a new context for its child
 * task. The context that it creates is a {@link HierarchicalContext}. The
 * parent context of the HierarchicalContext is the context of the
 * ModelHierarchicalContextManager, so if the child task does not find a variable in its
 * context, the context of the ModelHierarchicalContextManager will be used instead.
 * <p>
 * The spawning and updating of the child task are carried out as usual.
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
	public class ModelHierarchicalContextManager : ModelDecorator
	{
		/**
	 * Constructor.
	 * 
	 * @param guard
	 *            the guard of the ModelHierarchicalContextManager, which may be null.
	 * @param child
	 *            the child of the ModelHierarchicalContextManager.
	 */

		public ModelHierarchicalContextManager(ModelTask guard, ModelTask child) : base(guard, child)
		{
		}

		/**
	 * Returns an ExecutionContextManager that knows how to run this
	 * ModelHierarchicalContextManager.
	 * 
	 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
	 *      ExecutionTask)
	 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionHierarchicalContextManager(this, executor, parent);
		}
	}
}
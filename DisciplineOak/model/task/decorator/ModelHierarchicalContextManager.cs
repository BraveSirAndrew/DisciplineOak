/**
 * A ModelHierarchicalContextManager is a decorator that creates a new context for its child
 * task. The context that it creates is a {@link HierarchicalContext}. The
 * parent context of the HierarchicalContext is the context of the
 * ModelHierarchicalContextManager, so if the child task does not find a variable in its
 * context, the context of the ModelHierarchicalContextManager will be used instead.
 * <p>
 * The spawning and updating of the child task are carried out as usual.
 * 
 
 * 
 */

using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Decorator;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Decorator
{
	[Serializable]
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
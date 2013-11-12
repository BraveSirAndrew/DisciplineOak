/**
 * A ModelSafeOutputContextManager is a decorator that creates a new context for
 * its child task. The context that it creates is a {@link SafeOutputContext},
 * and the input context that the SafeOutputContext receives is that of the
 * ModelSafeOutputContextManager.
 * <p>
 * The spawning and updating of the child task are carried out as usual.
 * 
 
 * 
 */

using System.Collections.Generic;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Decorator;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Decorator
{
	public class ModelSafeOutputContextManager : ModelDecorator
	{
		/**
	 * The list of output variables of the SafeOutputContext.
	 */
		private readonly List<string> outputVariables;

		/**
	 * Constructor.
	 * 
	 * @param guard
	 *            the guard of the ModelSafeOutputContextManager, which may be
	 *            null.
	 * @param child
	 *            the child of the ModelSafeOutputContextManager.
	 * @param outputVariables
	 *            the list of output variables of the SafeOutputContext that is
	 *            created.
	 */

		public ModelSafeOutputContextManager(ModelTask guard, List<string> outputVariables, ModelTask child)
			: base(guard, child)
		{
			this.outputVariables = outputVariables;
		}

		/**
	 * Returns an ExecutionSafeOutputContextManager that knows how to run this
	 * ModelSafeOutputContextManager.
	 * 
	 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
	 *      ExecutionTask)
	 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionSafeOutputContextManager(this, executor, parent);
		}

		/**
	 * Returns a list with the set of output variables of the SafeOutputContext.
	 * The list cannot be modified.
	 * 
	 * @return a list with the set of output variables of the SafeOutputContext.
	 */

		public List<string> getOutputVariables()
		{
			return outputVariables;
		}
	}
}
/**
 * A ModelFailure represents a task that always fails.
 */

using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Leaf;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.Leaf.Action;

namespace DisciplineOak.Model.Task.Leaf
{
	[Serializable]
	public class ModelFailure : ModelAction
	{
		public ModelFailure(ModelTask guard) : base(guard)
		{
		}

		/**
	 * Returns an {@link ExecutionFailure} that knows how to run this
	 * ModelFailure.
	 * 
	 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
	 *      jbt.execution.core.ExecutionTask)
	 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionFailure(this, executor, parent);
		}
	}
}
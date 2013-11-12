﻿/**
 * A ModelFailure represents a task that always fails.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.leaf;
using DisciplineOak.model.Core;

namespace DisciplineOak.model.Task.Leaf
{
	public class ModelFailure : ModelLeaf
	{
		/**
	 * Constructor.
	 * 
	 * @param guard
	 *            the guard of the ModelFailure, which may be null.
	 */

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
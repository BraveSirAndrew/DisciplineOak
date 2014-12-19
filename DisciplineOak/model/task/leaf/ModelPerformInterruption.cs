/**
 * A ModelPerformInterruption is a task that interacts with a ModelInterrupter
 * decorator, interrupting it when it (the ModelPerformInterruption) is spawned.
 * A ModelPerformInterruption always succeeds when it is spawned. When the
 * ModelInterrupter gets interrupted, the status code it returns is also set by
 * the ModelPerformInterruption.
 * 
 
 * 
 */

using System;
using System.Collections.Generic;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Leaf;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.Decorator;
using DisciplineOak.Model.Task.Leaf.Action;

namespace DisciplineOak.Model.Task.Leaf
{
	[Serializable]
	public class ModelPerformInterruption : ModelAction
	{
		private Status _desiredResult;

		/**
		 * Constructor.
		 * 
		 * @param guard
		 *            the guard of the ModelPerformInterruption, which may be null.
		 * @param interrupter
		 *            the ModelInterrupter that this ModelPerformInterruption will
		 *            interrupt. May be null.
		 * @param desiredResult
		 *            the result that the ModelInterrupter should return in case it
		 *            is interrupted.
		 */

		public ModelPerformInterruption(ModelTask guard, Status desiredResult)
			: base(guard)
		{
			Interrupters = new List<ModelInterrupter>();
			_desiredResult = desiredResult;
		}

		public List<ModelInterrupter> Interrupters { get; set; }

		/**
		 * Returns the result that the ModelInterrupter should return in case it is
		 * interrupted.
		 * 
		 * @return e result that the ModelInterrupter should return in case it is
		 *         interrupted.
		 */
		public Status DesiredResult
		{
			get { return _desiredResult; }
			set { _desiredResult = value; }
		}

		/**
		 * Returns an ExecutionPerformInterruption that is able to run this
		 * ModelPerformInterruption.
		 * 
		 */
		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionPerformInterruption(this, executor, parent);
		}
	}
}
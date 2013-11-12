/**
 * A ModelPerformInterruption is a task that interacts with a ModelInterrupter
 * decorator, interrupting it when it (the ModelPerformInterruption) is spawned.
 * A ModelPerformInterruption always succeeds when it is spawned. When the
 * ModelInterrupter gets interrupted, the status code it returns is also set by
 * the ModelPerformInterruption.
 * 
 
 * 
 */

using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Leaf;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.Decorator;

namespace DisciplineOak.Model.Task.Leaf
{
	public class ModelPerformInterruption : ModelLeaf
	{
		private readonly Status _desiredResult;

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

		public ModelPerformInterruption(ModelTask guard, ModelInterrupter interrupter, Status desiredResult)
			: base(guard)
		{
			Interrupter = interrupter;
			_desiredResult = desiredResult;
		}

		public ModelInterrupter Interrupter { get; set; }

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
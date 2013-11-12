/**
 * An ModelInterrupter is a decorator that controls the termination of a child
 * task. An ModelInterrupter simply lets its child task run normally. If the
 * child returns a result, the ModelInterrupter will return it. However, the
 * ModelInterrupter can be asked to terminate the child task and return an
 * specified status when done so.
 * 
 
 * 
 */

using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Decorator;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Decorator
{
	public class ModelInterrupter : ModelDecorator
	{
		/**
	 * Constructor.
	 * <p>
	 * Constructs a ModelInterrupter with one child.
	 * 
	 * @param guard
	 *            the guard of the ModelInterrupter, which may be null.
	 * @param child
	 *            the child of the ModelInterrupter.
	 */

		public ModelInterrupter(ModelTask guard, ModelTask child) : base(guard, child)
		{
		}

		/**
	 * Returns an ExecutionInterrupter that is able to run this
	 * ModelInterrupter.
	 * 
	 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
	 *      ExecutionTask)
	 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionInterrupter(this, executor, parent);
		}
	}
}
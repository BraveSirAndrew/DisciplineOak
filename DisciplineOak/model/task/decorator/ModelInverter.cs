/**
 * ModelInverter is a decorator used to invert the status code returned by its
 * child.
 * <p>
 * When the decorated task finishes, its status code gets inverted according to:
 * 
 * <ul>
 * <li><code>Status.SUCCESS</code> -> <code>Status.FAILURE</code>.
 * <li><code>Status.FAILURE</code> -> <code>Status.SUCCESS</code>.
 * <li><code>Status.TERMINATED</code> -> <code>Status.SUCCESS</code>.
 * </ul>
 * 
 * If the child task has not finished yet, the ModelInverter returns
 * <code>Status.RUNNING</code> (that is, <code>Status.RUNNING</code> is not
 * inverted).
 * 
 
 * 
 */

using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Decorator;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Decorator
{
	public class ModelInverter : ModelDecorator
	{
		/**
	 * Constructor.
	 * 
	 * @param guard
	 *            the guard of the ModelInverter, which may be null.
	 * @param child
	 *            the child task to invert.
	 */

		public ModelInverter(ModelTask guard, ModelTask child) : base(guard, child)
		{
		}

		/**
	 * Returns an ExecutionInverter that is able to run this ModelInverter.
	 * 
	 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
	 *      ExecutionTask)
	 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionInverter(this, executor, parent);
		}
	}
}
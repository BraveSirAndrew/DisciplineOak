/**
 * ModelParallel is a task that runs all its children simultaneously. A
 * ModelParallel is constantly checking the evolution of its children.
 * <p>
 * The parallel task has a policy that defines the way it behaves. There are two
 * policies for parallel:
 * <ul>
 * <li>{@link ParallelPolicy#SEQUENCE_POLICY}: meaning the parallel behaves like
 * a sequence task, that is, it fails as soon as one of its children fail, and
 * it only succeeds if all of its children succeed. Otherwise it is running.
 * <li>{@link ParallelPolicy#SELECTOR_POLICY}: meaning the parallel behaves like
 * a selector task, that is, it succeeds as soon as one of its children succeeds,
 * and it only fails of all if its children fail. Otherwise it is running.
 * </ul>
 * 
 
 * 
 */

using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Composite;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Composite
{
	public class ModelParallel : ModelComposite
	{
		/** Policy of this ModelParallel task. */
		private readonly ParallelPolicy _policy;

		/**
		 * Creates a ModelParallel task with a guard, a policy and a list of
		 * children to run. A ModelParallel must have at least one child.
		 * 
		 * @param guard
		 *            the guard, which may be null.
		 * @param policy
		 *            the policy for the ModelParallel.
		 * @param children
		 *            the list of children. Must have at least one element.
		 */
		public ModelParallel(ModelTask guard, ParallelPolicy policy, params ModelTask[] children) : base(guard, children)
		{
			this._policy = policy;
		}

		/**
		 * Returns the policy of this ModelParallel.
		 * 
		 * @return the policy of this ModelParallel.
		 */
		public ParallelPolicy Policy
		{
			get { return _policy; }
		}

		/**
		 * Returns an ExecutionParallel that can run this ModelParallel.
		 * 
		 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
		 *      ExecutionTask)
		 */
		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionParallel(this, executor, parent);
		}
	}
}
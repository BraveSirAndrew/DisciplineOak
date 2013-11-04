/**
 * This class represents a task with one or more children, only one being
 * evaluated.
 * <p>
 * A ModelStaticPriorityList has a current active child, which is the task that
 * is being evaluated. The very first time the ModelStaticPriorityList is
 * spawned, the active child is set to the left most task whose guard is
 * evaluated to true. From then on, that child will run as normal, and the
 * ModelStaticPriorityList will finish as soon as its child finishes.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using OhBehave.Execution.Core;
using OhBehave.execution.core;
using OhBehave.Execution.Task.composite;
using OhBehave.Model.Core;

namespace OhBehave.Model.Task.composite
{
	public class ModelStaticPriorityList : ModelComposite
	{
		/**
	 * Creates a ModelStaticPriorityList task with a guard, and a list of
	 * children to run. A ModelStaticPriorityList must have at least one child.
	 * 
	 * @param guard
	 *            the guard, which may be null.
	 * @param children
	 *            the list of children. Must have at least one element.
	 */

		public ModelStaticPriorityList(ModelTask guard, params ModelTask[] children) : base(guard, children)
		{
		}

		/**
	 * Returns an ExecutionStaticPriorityList that is able to run this
	 * ModelStaticPriorityList.
	 * 
	 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
	 *      ExecutionTask)
	 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionStaticPriorityList(this, executor, parent);
		}
	}
}
/**
 * This class represents a task with one or more children, only one being
 * evaluated.
 * <p>
 * A ModelDynamicPriorityList has a current active child, which is the task that
 * is being evaluated. The very first time the ModelDynamicPriorityList is
 * spawned, the active child is set to the left most task whose guard is
 * evaluated to true. However, the current active task may change when the task
 * is ticked, according to the guards of the other tasks: if there is a task to
 * the left of the current active task whose guard is true, the latter is
 * terminated, and the new current active task is set to the former. In case
 * there are several tasks to the left of the current active task whose guards
 * are evaluated to true, the current active task will be the left most one.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using OhBehave.Execution.Core;
using OhBehave.execution.core;
using OhBehave.Execution.Task.composite;
using OhBehave.Model.Core;
using OhBehave.Model.Task.composite;

namespace OhBehave.model.task.composite
{
	public class ModelDynamicPriorityList : ModelComposite
	{
		/**
	 * Creates a ModelDynamicPriorityList task with a guard, and a list of
	 * children to run. A ModelDynamicPriorityList must have at least one child.
	 * 
	 * @param guard
	 *            the guard, which may be null.
	 * @param children
	 *            the list of children. Must have at least one element.
	 */

		public ModelDynamicPriorityList(ModelTask guard, params ModelTask[] children) : base(guard, children)
		{
		}

		/**
	 * Returns an ExecutionDynamicPriorityList that is able to run this
	 * ModelDynamicPriorityList.
	 * 
	 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
	 *      ExecutionTask)
	 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionDynamicPriorityList(this, executor, parent);
		}
	}
}
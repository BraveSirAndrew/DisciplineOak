/**
 * A ModeSequence is a task with one or more children which are evaluated
 * sequentially.
 * <p>
 * A ModeSequence has an active child, which is the child task currently being
 * evaluated. If the execution of the current child finishes successfully, the
 * next child of the sequence is spawned and evaluated. However, if the
 * execution of the currently active child ends in failure, the whole
 * ModeSequence also fails.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Composite;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.composite
{
	public class ModelSequence : ModelComposite
	{
		/**
	 * Constructor.
	 * <p>
	 * Constructs a ModeSequence with some children. A ModeSequence must have at
	 * least one child.
	 * 
	 * @param guard
	 *            the guard of the ModeSequence, which may be null.
	 * @param children
	 *            the list of children. Must have at least one element.
	 */

		public ModelSequence(ModelTask guard, params ModelTask[] children) : base(guard, children)
		{
		}

		/**
	 * Returns an ExecutionSequence that can run this ModelSequence.
	 * 
	 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
	 *      ExecutionTask)
	 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionSequence(this, executor, parent);
		}
	}
}
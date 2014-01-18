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
 
 * 
 */

using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Composite;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Composite
{
	public class ModelSequence : ModelComposite
	{

		///	 Constructs a ModeSequence with some children. A ModeSequence must have at
		///	 least one child.
		public ModelSequence(ModelTask guard, params ModelTask[] children)
			: this(guard, null, children)
		{
		}

		public ModelSequence(ModelTask guard, string name, params ModelTask[] children)
			: base(guard, name, children)
		{
		}


		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionSequence(this, executor, parent);
		}
	}
}
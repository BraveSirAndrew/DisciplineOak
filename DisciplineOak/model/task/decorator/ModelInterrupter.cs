using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Decorator;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Decorator
{
	/// <summary>
	/// 
	///An ModelInterrupter is a decorator that controls the termination of a child
	///task. An ModelInterrupter simply lets its child task run normally. If the
	///child returns a result, the ModelInterrupter will return it. However, the
	///ModelInterrupter can be asked to terminate the child task and return an
	///specified status when done so.
	/// </summary>
	[Serializable]
	public class ModelInterrupter : ModelDecorator
	{
		public ModelInterrupter(ModelTask guard, ModelTask child)
			: base(guard, child)
		{
			NumInterrupterBranchTicks = 10;
		}

		/// <summary>
		/// The number of times to tick the interrupter branch. The entire branch should complete in less than or equal to the
		/// number specified here. The number of ticks required to complete a branch is not a straightforward calculation, but 
		/// generally each leaf node will be spawned on one tick and run on the next one. In practice, if part of your interruptor
		/// branch doesn't seem to be running, increase the number of ticks until it all runs, but keep in mind that the 
		/// behaviour tree will hog the processor/thread during those ticks, so keep it practical.
		/// </summary>
		public int NumInterrupterBranchTicks { get; set; }

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionInterrupter(this, executor, parent);
		}
	}
}
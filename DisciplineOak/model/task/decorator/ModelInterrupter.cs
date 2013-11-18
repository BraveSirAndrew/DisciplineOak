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
	public class ModelInterrupter : ModelDecorator
	{
		public ModelInterrupter(ModelTask guard, ModelTask child)
			: base(guard, child)
		{
		}
		
		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionInterrupter(this, executor, parent);
		}
	}
}
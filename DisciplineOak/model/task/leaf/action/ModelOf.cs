using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Leaf.Action;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Leaf.Action
{
	/// <summary>
	/// Use this as the default Model using generic types like ModelOf<YourExecutionAction> 
	/// If you need to have a more complex model, then you should create a new class tht inherits from 
	/// ModelAction
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ModelOf<T> : ModelAction where T : ExecutionAction
	{

		public ModelOf(ModelTask guard)
			: base(guard)
		{
		}

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			var executionAction = (T)Activator.CreateInstance(typeof(T), this, executor, parent);
			return executionAction;
		}
	}
}
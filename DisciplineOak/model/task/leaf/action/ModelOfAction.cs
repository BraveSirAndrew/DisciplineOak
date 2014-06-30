using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Leaf.Action
{
	/// <summary>
	/// Use this as the default Model using generic types like ModelOf<YourExecutionAction> 
	/// If you need to have a more complex model, then you should create a new class that inherits from 
	/// ModelAction
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class ModelOfAction<T> : ModelAction where T : ExecutionTask
	{
		private T _executionAction;

		public ModelOfAction(ModelTask guard)
			: base(guard)
		{
		}

		public ModelOfAction(ModelTask guard, string name)
			: base(guard, name)
		{
		}

		public T Action {get { return _executionAction; }}

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			_executionAction = (T)Activator.CreateInstance(typeof(T), this, executor, parent);
			return _executionAction;
		}
	}
}
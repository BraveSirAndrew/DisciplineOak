using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.Leaf.Condition;

namespace DisciplineOak.Model.Task.Leaf.Action
{
	[Serializable]
	public class ModelOfCondition<T> : ModelCondition where T : ExecutionTask
	{
		private T _executionAction;

		public ModelOfCondition(ModelTask guard)
			: base(guard)
		{
		}

		public ModelOfCondition(ModelTask guard, string name)
			: base(guard, name)
		{
		}

		public T Action { get { return _executionAction; } }

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			_executionAction = (T)Activator.CreateInstance(typeof(T), this, executor, parent);
			return _executionAction;
		}
	}
}
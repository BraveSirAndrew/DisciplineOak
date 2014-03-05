using DisciplineOak.Execution.Core;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Tests
{
	public class TestTask : ModelTask
	{
		public TestTask(ModelTask guard, params ModelTask[] children) : base(guard, children)
		{
		}

		public TestTask(ModelTask guard, string name, params ModelTask[] children)
			: base(guard, name, children)
		{
		}

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return null;
		}
	}
}
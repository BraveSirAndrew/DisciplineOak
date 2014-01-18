using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Composite;
using DisciplineOak.Model.Core;
using NUnit.Framework;

namespace DisciplineOak.Tests
{
	[TestFixture]
	public class ModelTaskTests
	{
		public class TheNameProperty
		{
			[Test]
			public void ShouldReturnTheNameOfTheClassIfNameHasntBeenSetExplicitly()
			{
				var task = new TestTask(null, new ModelTask[0]);

				Assert.AreEqual("TestTask", task.Name);
			}

			[Test]
			public void IncludesTheGenericTypesInTheName()
			{
				var task = new GenericTestTask<ExecutionSequence, ExecutionComposite>(null, new ModelTask[0]);

				Assert.AreEqual("GenericTestTask<ExecutionSequence, ExecutionComposite>", task.Name);
			}

			[Test]
			public void ShouldReturnTheExplicitlySetNameIfItHasBeenSet()
			{
				var task = new TestTask(null, "TurtlePants", new ModelTask[0]);

				Assert.AreEqual("TurtlePants", task.Name);
			}

			private class TestTask : ModelTask
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

			private class GenericTestTask<T, U> : TestTask where T:ExecutionTask
			{
				public GenericTestTask(ModelTask guard, params ModelTask[] children)
					: base(guard, children)
				{
				}

				public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
				{
					return null;
				}
			}
		}
	}
}

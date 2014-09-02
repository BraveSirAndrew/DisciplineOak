using DisciplineOak.Execution.Context;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Core.Events;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.Leaf;
using Moq;
using NUnit.Framework;

namespace DisciplineOak.Tests
{
	[TestFixture]
	public class ExecutionTaskTests
	{
		[TestFixture]
		public class TheConstructor
		{
			[Test]
			public void SetsAlwaysFailBasedOnValueInModelTask()
			{
				var modelTask = new Mock<ModelTask>((ModelTask)null);
				modelTask.Object.AlwaysFail = true;
				var task = new Mock<ExecutionTask>(modelTask.Object, new BTExecutor(new ModelSuccess(null)), (ExecutionTask)null);

				Assert.IsTrue(task.Object.AlwaysFail);
			}
		}

		[TestFixture]
		public class TheTickMethod
		{
			[Test]
			public void ReturnsFailureWhenAlwaysFailIsTrueEvenIfInternalTickReturnsSuccess()
			{
				var task = new StubExecutionTask(null, new BTExecutor(new ModelSuccess(null)), null) {AlwaysFail = true};
				task.Spawn(new BasicContext());
				Assert.AreEqual(Status.Failure, task.Tick());
			}

			private class StubExecutionTask : ExecutionTask
			{
				public StubExecutionTask(ModelTask modelTask, IBTExecutor executor, ExecutionTask parent) : base(modelTask, executor, parent)
				{
				}

				public override void StatusChanged(TaskEvent e)
				{
					
				}

				protected override void InternalSpawn()
				{
					
				}

				protected override Status InternalTick()
				{
					return Status.Success;
				}

				protected override ITaskState StoreState()
				{
					return null;
				}

				protected override ITaskState StoreTerminationState()
				{
					return null;
				}

				protected override void RestoreState(ITaskState state)
				{
				}

				protected override void InternalTerminate()
				{
				}
			}
		}
	}
}

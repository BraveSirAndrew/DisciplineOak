using DisciplineOak.Execution.Context;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Core.Events;
using DisciplineOak.Execution.Task.Decorator;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.Decorator;
using DisciplineOak.Model.Task.Leaf;
using NUnit.Framework;

namespace DisciplineOak.Tests.Execution.Task.Decorator
{
	[TestFixture]
	public class ExecutionInterrupterTests
	{
		[TestFixture]
		public class TheInterruptMethod
		{
			[Test]
			public void TicksTheInterruptBranchTheSpecifiedNumberOfTimes()
			{
				var model = new ModelInterrupter(null, 
					new ModelSuccess(null)
					{
						Interrupter = new InterrupterBranchTask(null)
					}) {NumInterrupterBranchTicks = 10};

				var context = new BasicContext();
				var executor = new ExecutionInterrupter(model, new BTExecutor(model, context), null);
				executor.Spawn(context);

				executor.Interrupt(Status.Success);

				Assert.AreEqual(10, TestInterrupterExecutor.RecordedTicks);
			}
		}

		protected class InterrupterBranchTask : ModelTask
		{
			public InterrupterBranchTask(ModelTask guard, params ModelTask[] children) : base(guard, children)
			{
			}

			public InterrupterBranchTask(ModelTask guard, string name, params ModelTask[] children) : base(guard, name, children)
			{
			}

			public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
			{
				return new TestInterrupterExecutor(this, executor, parent);
			}
		}

		protected class TestInterrupterExecutor : ExecutionTask
		{
			public static int RecordedTicks { get; set; }

			public TestInterrupterExecutor(ModelTask modelTask, IBTExecutor executor, ExecutionTask parent) 
				: base(modelTask, executor, parent)
			{

			}

			public override void StatusChanged(TaskEvent e)
			{
				
			}

			protected override void InternalSpawn()
			{
				Executor.RequestInsertionIntoList(BTExecutor.BTExecutorList.Tickable, this);
			}

			protected override Status InternalTick()
			{
				RecordedTicks++;
				return Status.Running;
			}

			protected override ITaskState StoreState()
			{
				return new TaskState();
			}

			protected override ITaskState StoreTerminationState()
			{
				return new TaskState();
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

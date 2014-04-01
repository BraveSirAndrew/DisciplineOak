using DisciplineOak.Debugger;
using DisciplineOak.Execution.Core;
using DisciplineOak.Model.Task.Composite;
using DisciplineOak.Model.Task.Leaf.Action;
using NUnit.Framework;
using Moq;

namespace DisciplineOak.Tests.Debugger
{
	[TestFixture]
	public class BehaviourDebuggerTests
	{
		private Mock<IBehaviourDebugger> mockDebugger;

		[SetUp]
		public void Setup()
		{
			mockDebugger = new Mock<IBehaviourDebugger>();
			BehaviourDebugger.SetActiveDebugger(mockDebugger.Object);
		}

		[Test]
		public void When_Ticked_then_Task_Logs_Tick_with_Debugger()
		{
			var modelSequence = new ModelSequence(null,
				new ModelOfAction<MyAction>(null),
				new ModelOfAction<MyAction>(null));
			var executor = BTExecutorFactory.CreateBTExecutor(modelSequence);

			executor.Tick();
			executor.Tick();
			mockDebugger.Verify(m => m.LogTick(It.IsAny<MyAction>()), Times.Exactly(1));
		}

		[Test]
		public void When_Ticked_then_Guard_Logs_Tick_with_Debugger()
		{
			var guard = new ModelOfAction<MyAction>(null);
			
			var modelSequence = new ModelDynamicPriorityList(null,
				new ModelOfAction<MyAction>(guard),
				new ModelOfAction<MyAction>(null));
			var executor = BTExecutorFactory.CreateBTExecutor(modelSequence);

			executor.Tick();
			mockDebugger.ResetCalls();
			guard.Action.ReturnStatusForInternalTick = Status.Success;
			executor.Tick();
			mockDebugger.Verify(m => m.LogTick(It.Is((ExecutionTask task) => task.ModelTask == guard)), Times.Exactly(1));
		}

		[Test]
		public void When_Debugger_is_null_then_does_not_throw()
		{
			BehaviourDebugger.SetActiveDebugger(null);
			var modelSequence = new ModelOfAction<MyAction>(null);
			var executor = BTExecutorFactory.CreateBTExecutor(modelSequence);

			Assert.DoesNotThrow(delegate
			{
				executor.Tick();
				executor.Tick();
			});

			mockDebugger.Verify(m => m.LogTick(It.IsAny<MyAction>()), Times.Never);
		}

		[Test]
		public void When_Debugger_Disabled_then_Logs_not_Received()
		{
			BehaviourDebugger.DisableDebugger();
			var modelSequence = new ModelOfAction<MyAction>(null);
			var executor = BTExecutorFactory.CreateBTExecutor(modelSequence);

			Assert.DoesNotThrow(delegate
			{
				executor.Tick();
				executor.Tick();
			});

			mockDebugger.Verify(m => m.LogTick(It.IsAny<MyAction>()), Times.Never);
		}
	}
}

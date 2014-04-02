using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Leaf.Action;

using DisciplineOak.Model.Task.Leaf.Action;

namespace DisciplineOak.Tests
{
	internal class MyAction : ExecutionAction
	{
		public MyAction(ModelAction modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			ReturnStatusForInternalTick = Status.Running;
		}

		protected override void InternalSpawn()
		{
			Executor.RequestInsertionIntoList(BTExecutor.BTExecutorList.Tickable, this);

			WasSpawned = true;
		}

		public bool WasSpawned { get; private set; }

		public Status ReturnStatusForInternalTick { get; set; }

		protected override Status InternalTick()
		{
			return ReturnStatusForInternalTick;
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
using System;
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
		}

		protected override void InternalSpawn()
		{
			Executor.RequestInsertionIntoList(BTExecutor.BTExecutorList.Tickable, this);

			WasSpawned = true;
		}

		public bool WasSpawned { get; private set; }

		protected override Status InternalTick()
		{
			return Status.Running;
		}

		protected override ITaskState StoreState()
		{
			throw new NotImplementedException();
		}

		protected override ITaskState StoreTerminationState()
		{
			throw new NotImplementedException();
		}

		protected override void RestoreState(ITaskState state)
		{

		}

		protected override void InternalTerminate()
		{

		}
	}
}
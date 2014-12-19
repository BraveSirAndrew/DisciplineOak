using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Core.Events;
using DisciplineOak.Model.Task.Decorator;

namespace DisciplineOak.Execution.Task.Decorator
{
	public class ExecutionInterrupter : ExecutionDecorator
	{

		private bool _interrupted;
		private ExecutionTask _executionChild;
		private Status _statusSet;

		///Creates an ExecutionInterrupter that is able to run a ModelInterrupter
		///task and that is managed by a BTExecutor.
		///
		///@param modelTask the ModelInterrupter that this ExecutionInterrupter is going to run.
		///@param executor the BTExecutor in charge of running this ExecutionInterrupter.
		///@param parent the parent ExecutionTask of this task.
		public ExecutionInterrupter(ModelInterrupter modelTask, IBTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			_interrupted = false;
		}


		///Spawns its child and registers itself into the list of interrupters of
		///the BTExecutor.
		///
		protected override void InternalSpawn()
		{
			_executionChild = ((ModelInterrupter)ModelTask).getChild().CreateExecutor(Executor, this);
			_executionChild.AddTaskListener(this);

			//Register the ExecutionInterrupter so that ExecutionPerformInterruption can find it.
			Executor.RegisterInterrupter(this);
			_executionChild.Spawn(Context);
		}

		///
		///Terminates the child task and unregister itself from the list of
		///interrupters of the BTExecutor.

		protected override void InternalTerminate()
		{
			///
			///Unregister the ExecutionInterrupter so that it is no longer available
			///to ExecutionPerformInterruption.
			///
			Executor.UnregisterInterrupter(this);
			///
			///It is important to cancel any request for insertion that this task
			///has. If the task has been interrupted, it will have requested to be
			///inserted into the list of tickable nodes. However, if it is then
			///terminated, we do not want it to be inserted into the list of
			///tickable nodes, so the request made in "interrupt()" must be
			///cancelled.
			///
			if (_interrupted)
			{
				Executor.CancelInsertionRequest(BTExecutor.BTExecutorList.Tickable, this);
			}

			_executionChild.Terminate();
		}

		///
		///If the ExecutionInterrupter has been interrupted, returns the status that
		///was passed to the {@link #interrupt(Status)} method. Otherwise, returns
		///the current status of the child task.

		protected override Status InternalTick()
		{
			if (_interrupted)
			{

				///Unregister the ExecutionInterrupter so that it is no longer
				///available to ExecutionPerformInterruption.
				Executor.UnregisterInterrupter(this);
				return _statusSet;
			}
			Status childStatus = _executionChild.Status;
			if (childStatus != Status.Running)
			{
				///
				///If the child has finished, unregister the
				///ExecutionInterrupter so that it is no longer available to
				///ExecutionPerformInterruption.
				///
				Executor.UnregisterInterrupter(this);
			}
			return childStatus;
		}


		protected override void RestoreState(ITaskState state)
		{
		}

		public override void StatusChanged(TaskEvent e)
		{
			Tick();
		}

		protected override ITaskState StoreState()
		{
			return null;
		}

		///
		///Interrupts the ExecutionInterrupter. This method causes the
		///ExecutionInterrupter to terminate its child and set the status that will
		///be returned to <code>status</code>. Also, it requests to be inserted into
		///the list of tickable nodes, since the terminated child will no longer
		///react to ticks after being terminated.
		///<p>
		///A task that have not been spawned cannot be interrupted. It should be
		///noted that if the task has already been terminated, this method does
		///nothing. Also, if the task has already been interrupted, this method does
		///nothing too.
		///
		///@param status
		///           the status that the ExecutionInterrupter will return.
		///


		public void Interrupt(Status status)
		{
			if (!_interrupted)
			{
				///
				///If the task has not been spawned, throw an exception.
				///
				if (!Spawned)
				{
					throw new Exception("Cannot interrupt an ExecutionInterrupter that has not been spawned");
				}

				///
				///Also it is important to note that that if the task has been
				///terminated it cannot be interrupted, since by doing so the task
				///would insert itself into the list of tickable nodes (see below),
				///which should not be done since the task has been terminated.
				///
				if (!HasTerminated)
				{
					if (status != Status.Failure && status != Status.Success)
					{
						throw new ArgumentException("The specified status is not valid. Must be either Status.FAILURE or Status.SUCCESS");
					}

					if (_executionChild.ModelTask.Interrupter != null)
					{
						var executor = new BTExecutor(_executionChild.ModelTask.Interrupter, Context);
						RunInterrupterBranch(executor, ((ModelInterrupter)ModelTask).NumInterrupterBranchTicks + 1);
					}

					_executionChild.Terminate();

					///It is very important for the ExecutionInterrupter to be
					///inserted into the list of tickable nodes. If not, after being
					///interrupted, it will not inform its parent about the
					///termination of its child. Keep in mind that after terminating
					///its child, the child will not react to ticks (actually it
					///will leave the list of tickable nodes in the next AI cycle),
					///so it has to be the interrupter itself that informs its
					///parent about termination.
					Executor.RequestInsertionIntoList(BTExecutor.BTExecutorList.Tickable, this);
					_interrupted = true;
					_statusSet = status;
				}
			}
		}

		protected override ITaskState StoreTerminationState()
		{
			return null;
		}

		public static void RunInterrupterBranch(IBTExecutor executor, int numTicks)
		{
			var counter = 0;
			do
			{
				executor.Tick();
				counter++;
			} while (executor.GetStatus() == Status.Running && counter < numTicks);
		}
	}
}
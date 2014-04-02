using DisciplineOak.Execution.Core;

namespace DisciplineOak.Debugger
{
	public static class BehaviourDebugger
	{
		public static IBehaviourDebugger ActiveDebugger { get; private set; }

		public static void SetActiveDebugger(IBehaviourDebugger debugger)
		{
			ActiveDebugger = debugger;
		}

		public static void DisableDebugger()
		{
			ActiveDebugger = null;
		}

		public static void LogTick(ExecutionTask task)
		{
			if (ActiveDebugger == null)
			{
				return;
			}

			ActiveDebugger.LogTick(task);
		}
	}
}
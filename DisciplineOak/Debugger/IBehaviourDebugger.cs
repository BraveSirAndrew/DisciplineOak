using DisciplineOak.Execution.Core;

namespace DisciplineOak.Debugger
{
	public interface IBehaviourDebugger
	{
		void LogTick(ExecutionTask task);
	}
}
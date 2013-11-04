/**
 * ExecutionVariableRenamer is the ExecutionTask that knows how to run a
 * {@link ModelVariableRenamer}.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using OhBehave.Execution.Core;
using OhBehave.Execution.core;
using OhBehave.execution.core;
using OhBehave.Model.Core;
using OhBehave.Model.Task.Leaf;

public class ExecutionVariableRenamer : ExecutionLeaf
{
	/** The name of the variable that must be renamed. */
	/** The new name for the variable that must be renamed. */
	private readonly string _newVariableName;
	private readonly string _variableName;

	/**
	 * Constructs an ExecutionVariableRenamer that knows how to run a
	 * ModelVariableRenamer.
	 * 
	 * @param modelTask
	 *            the ModelVariableRenamer to run.
	 * @param executor
	 *            the BTExecutor in charge of running this
	 *            ExecutionVariableRenamer.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

	public ExecutionVariableRenamer(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
		: base(modelTask, executor, parent)
	{
		if (!(modelTask is ModelVariableRenamer))
		{
			throw new ArgumentException("The ModelTask must subclass ModelVariableRenamer but it inherits from " +
			                            modelTask.GetType().Name);
		}

		_variableName = ((ModelVariableRenamer) modelTask).VariableName;
		_newVariableName = ((ModelVariableRenamer) modelTask).NewVariableName;
	}

	/**
	 * Renames the variable in the context.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalSpawn()
	 */

	protected override void InternalSpawn()
	{
		Executor.RequestInsertionIntoList(BTExecutor.BTExecutorList.Tickable, this);
		var variable = Context[_variableName];
		Context.ClearVariable(_variableName);
		Context.SetVariable(_newVariableName, variable);
	}

	/**
	 * Returns {@link Status#SUCCESS}.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTick()
	 */

	protected override Status InternalTick()
	{
		return Status.Success;
	}

	/**
	 * Does nothing.
	 * 
	 * @see jbt.execution.core.ExecutionTask#storeState()
	 */

	protected override ITaskState StoreState()
	{
		return null;
	}

	/**
	 * Does nothing.
	 * 
	 * @see jbt.execution.core.ExecutionTask#storeTerminationState()
	 */

	protected override ITaskState StoreTerminationState()
	{
		return null;
	}

	/**
	 * Does nothing.
	 * 
	 * @see jbt.execution.core.ExecutionTask#restoreState(jbt.execution.core.ITaskState)
	 */

	protected override void RestoreState(ITaskState state)
	{
	}

	/**
	 * Does nothing.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTerminate()
	 */

	protected override void InternalTerminate()
	{
	}
}
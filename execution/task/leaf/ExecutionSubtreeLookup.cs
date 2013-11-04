/**
 * ExecutionSubtreeLookup is the ExecutionTask that knows how to run a
 * ModelExecutionSubtreeLookup.
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

public class ExecutionSubtreeLookup : ExecutionLeaf
{
	/**
	 * Behaviour tree that is retrieved from the context and that is going to be
	 * run.
	 */
	/**
	 * The tree that is actually being run (constructed from {@link #treeToRun}
	 * ).
	 */
	private ExecutionTask executionTree;
	/** Flag that tells if the tree could be retrieved from the context. */
	private bool treeRetrieved;
	private ModelTask treeToRun;

	/**
	 * Constructs an ExecutionSubtreeLookup that knows how to run a
	 * ModelSubtreeLookup.
	 * 
	 * @param modelTask
	 *            the ModelSubtreeLookup to run.
	 * @param executor
	 *            the BTExecutor that will manage this ExecutionSubtreeLookup.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

	public ExecutionSubtreeLookup(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
		: base(modelTask, executor, parent)
	{
		if (!(modelTask is ModelSubtreeLookup))
		{
			throw new ArgumentException("The ModelTask must subclass ModelSubtreeLookup but it inherits from " +
			                            modelTask.GetType().Name);
		}
	}

	/**
	 * This method first retrieve from the context the tree (ModelTask) that is
	 * going to be emulated by this task. Then, it creates its corresponding
	 * executor and finally spawns it. If the tree cannot be found in the
	 * context, does nothing.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalSpawn()
	 */

	protected override void InternalSpawn()
	{
		/* Retrieve the tree to run from the context. */
		treeToRun = Context.GetBT(
			((ModelSubtreeLookup) ModelTask).TreeName);

		if (treeToRun == null)
		{
			treeRetrieved = false;
			/*
			 * Must request to be inserted into the list of tickable nodes,
			 * since no tree has been retrieved and as a result it must be the
			 * task the one continuin the work.
			 */
			Executor.RequestInsertionIntoList(BTExecutor.BTExecutorList.Tickable, this);
//			System.err.println("Could not retrieve tree "
//					+ ((ModelSubtreeLookup) this.getModelTask()).getTreeName()
//					+ " from the context. Check if the context has been properly initialized.");
		}
		else
		{
			treeRetrieved = true;
			/* Compute positions for the retrieved tree. */
			treeToRun.ComputePositions();

			executionTree = treeToRun.CreateExecutor(Executor, this);
			executionTree.AddTaskListener(this);
			executionTree.Spawn(Context);
		}
	}

	/**
	 * Just terminates the tree that it is emulating, or does nothing if the
	 * tree could not be retrieved.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTerminate()
	 */

	protected override void InternalTerminate()
	{
		if (treeRetrieved)
		{
			executionTree.Terminate();
		}
	}

	/**
	 * Returns the status of the tree it is running, or null if the tree could
	 * not be retrieved.
	 * 
	 * @see jbt.execution.core.ExecutionTask#internalTick()
	 */

	protected override Status InternalTick()
	{
		if (treeRetrieved)
		{
			return executionTree.GetStatus();
		}
		return Status.Failure;
	}

	/**
	 * Does nothing.
	 * 
	 * @see jbt.execution.core.ExecutionTask#restoreState(ITaskState)
	 */

	protected override void RestoreState(ITaskState state)
	{
	}

	/**
	 * Just calls {@link #tick()} to make the task evolve.
	 * 
	 * @see jbt.execution.task.leaf.ExecutionLeaf#statusChanged(jbt.execution.core.event.TaskEvent)
	 */

	public override void StatusChanged(TaskEvent e)
	{
		Tick();
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
}
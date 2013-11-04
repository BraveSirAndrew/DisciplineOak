/**
 * Base class for all the ExecutionTask subclasses that are able to run
 * composite tasks (that is, classes that inherit from ModelComposite).
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using OhBehave.Execution.Core;
using OhBehave.execution.core;
using OhBehave.Model.Core;
using OhBehave.Model.Task.composite;

namespace OhBehave.Execution.Task.composite
{
	public abstract class ExecutionComposite : ExecutionTask
	{
		/**
	 * Creates an ExecutionComposite that is able to run a particular
	 * ModelComposite task.
	 * 
	 * @param modelTask
	 *            the ModelComposite task to run.
	 * @param executor
	 *            the BTExecutor that will manage this ExecutionComposite.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

		public ExecutionComposite(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelComposite))
			{
				throw new ArgumentException("The ModelTask must subclass " + typeof (ModelComposite).Name + " but it inherits from " +
				                            modelTask.GetType().Name);
			}
		}
	}
}
/**
 * Base class for all the ExecutionTask subclasses that are able to run
 * composite tasks (that is, classes that inherit from ModelComposite).
 * 
 
 * 
 */

using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Model.Core;
using DisciplineOak.Model.Task.Composite;

namespace DisciplineOak.Execution.Task.Composite
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

		protected ExecutionComposite(ModelTask modelTask, IBTExecutor executor, ExecutionTask parent)
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
/**
 * Base class for all the ExecutionTask subclasses that are able to run
 * decorator tasks (that is, classes that inherit from ModelDecorator).
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.model.Core;
using DisciplineOak.model.Task.Decorator;

namespace DisciplineOak.Execution.Task.decorator
{
	public abstract class ExecutionDecorator : ExecutionTask
	{
		/**
	 * Creates an ExecutionDecorator that is able to run a particular
	 * ModelDecorator task.
	 * 
	 * @param modelTask
	 *            the ModelDecorator task to run.
	 * @param executor
	 *            the BTExecutor that will manage this ExecutionDecorator.
	 * @param parent
	 *            the parent ExecutionTask of this task.
	 */

		public ExecutionDecorator(ModelTask modelTask, BTExecutor executor, ExecutionTask parent)
			: base(modelTask, executor, parent)
		{
			if (!(modelTask is ModelDecorator))
			{
				throw new ArgumentException("The ModelTask must subclass ModelDecorator but it inherits from " +
				                            modelTask.GetType().Name);
			}
		}
	}
}
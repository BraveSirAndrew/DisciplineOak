/**
 * Limit is a decorator that limits the number of times a task can be executed.
 * This decorator is used when a task (the child of the decorator) must be run a
 * maximum number of times. When the maximum number of times is exceeded, the
 * decorator will fail forever on.
 * 
 
 * 
 */

using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Decorator;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Decorator
{
	[Serializable]
	public class ModelLimit : ModelDecorator
	{
		/** Maximum number of times that the decorated task can be run. */
		private readonly int maxNumTimes;

		/**
	 * Constructor.
	 * 
	 * @param guard
	 *            the guard of the ModelLimit, which may be null.
	 * @param maxNumTimes
	 *            the maximum number of times that <code>child</code> will be
	 *            run.
	 * @param child
	 *            the child of this task.
	 */

		public ModelLimit(ModelTask guard, int maxNumTimes, ModelTask child) : base(guard, child)
		{
			this.maxNumTimes = maxNumTimes;
		}

		/**
	 * Returns an ExecutionLimit that knows how to run this ModelLimit.
	 * 
	 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
	 *      ExecutionTask)
	 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionLimit(this, executor, parent);
		}

		/**
	 * Returns the maximum number of times that the decorated task can be run.
	 * 
	 * @return the maximum number of times that the decorated task can be run.
	 */

		public int getMaxNumTimes()
		{
			return maxNumTimes;
		}
	}
}
﻿/**
 * The BTExecutorFactory implements the simple factory pattern, and allows
 * clients of the framework to create instances of {@link IBTExecutor} objects
 * that will run specific behaviour trees.
 * 
 
 * 
 */

using DisciplineOak.Model.Core;

namespace DisciplineOak.Execution.Core
{
	public class BTExecutorFactory
	{
		/**
	 * Creates an IBTExecutor that is able to run a specific behaviour tree. The
	 * input context is also specified.
	 * 
	 * @param treeToRun
	 *            the behaviour tree that the returned IBTExecutor will run,
	 * @param context
	 *            the input context to be used by the behaviour tree.
	 * @return an IBTExecutor to run the tree <code>treeToRun</code>.
	 */

		public static IBTExecutor CreateBTExecutor(ModelTask treeToRun,
			IContext context)
		{
			return new BTExecutor(treeToRun, context);
		}

		/**
	 * Creates an IBTExecutor that is able to run a specific behaviour tree. A
	 * new empty context is created for the tree.
	 * 
	 * @param treeToRun
	 *            the behaviour tree that the returned IBTExecutor will run,
	 * @return an IBTExecutor to run the tree <code>treeToRun</code>.
	 */

		public static IBTExecutor CreateBTExecutor(ModelTask treeToRun)
		{
			return new BTExecutor(treeToRun);
		}
	}
}
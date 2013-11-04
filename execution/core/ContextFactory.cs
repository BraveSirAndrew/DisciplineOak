/**
 * The ContextFactory implements the simple factory pattern, and allows clients
 * of the framework to create instances of {@link IContext} objects that can be
 * used when running behaviour trees.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System.Collections.Generic;
using DisciplineOak.Execution.Context;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Execution.Core
{
	public class ContextFactory
	{
		/**
	 * Creates a new empty context (with no variables) that contains all the
	 * behaviour trees specified in <code>library</code>.
	 * 
	 * @param library
	 *            the set of behaviour trees that the returned IContext will
	 *            contain.
	 * @return a new empty context that contains all the behaviour trees
	 *         specified in <code>library</code>.
	 */

		public static IContext CreateContext(IBTLibrary library)
		{
			var result = new BasicContext();
			result.AddBTLibrary(library);
			return result;
		}

		/**
	 * Creates a new empty context (with no variables) that contains all the
	 * behaviour trees in the libraries <code>libraries</code>.
	 * 
	 * @param libraries
	 *            the list of libraries whose behaviour trees this context will
	 *            contain.
	 * @return a new empty context that contains all the behaviour trees in the
	 *         libraries <code>libraries</code>.
	 */

		public static IContext CreateContext(List<IBTLibrary> libraries)
		{
			var result = new BasicContext();

			foreach (var btLibrary in libraries)
			{
				result.AddBTLibrary(btLibrary);
			}

			return result;
		}

		/**
		 * Creates a new empty context (with no variables in it) that contains all
		 * the behaviour trees in <code>behaviourTrees</code>. The name of the trees
		 * are specified in <code>names</code>, so, for instance, the i-th element
		 * in <code>names</code> represents the name of the i-th tree in
		 * <code>behaviourTrees</code>.
		 * 
		 * @param behaviourTrees
		 *            the list with the trees that the context will contain.
		 * @param names
		 *            the list with the names of the trees.
		 * @return a new empty context that contains all the behaviour trees in the
		 *         list <code>behaviourTrees</code>.
		 */
		public static IContext CreateContext(List<ModelTask> behaviourTrees, List<string> names)
		{
			var result = new BasicContext();


			var treesIterator = behaviourTrees.GetEnumerator();
			var namesIterator = names.GetEnumerator();

			while (treesIterator.MoveNext() && namesIterator.MoveNext())
			{
				result.AddBT(namesIterator.Current, treesIterator.Current);
			}

			return result;
		}

		/**
	 * Creates a new empty context (with no variables in it).
	 * 
	 * @return a new empty context.
	 */

		public static IContext CreateContext()
		{
			return new BasicContext();
		}
	}
}
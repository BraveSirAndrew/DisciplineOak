/**
 * The BTLibraryFactory implements the simple factory pattern, and allows
 * clients of the framework to create instances of {@link IBTLibrary} composed
 * of behaviour trees.
 * 
 
 * 
 */

using System.Collections.Generic;
using DisciplineOak.Execution.Context;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Execution.Core
{
	public class BTLibraryFactory
	{
		/**
	 * Creates a BT library that contains all the BTs contained in the libraries
	 * of <code>libraries</code>. If several trees are referenced by the same
	 * name, only the last one (according to its order in the input libraries)
	 * will remain.
	 * 
	 * @param libraries
	 *            the list with all the libraries whose BTs will contain the
	 *            returned BT library.
	 * @return a BT library that contains all the BTs contained in the libraries
	 *         of <code>libraries</code>.
	 */

		public static IBTLibrary createBTLibrary(List<IBTLibrary> libraries)
		{
			var result = new GenericBTLibrary();

			foreach (var btLibrary in libraries)
			{
				result.addBTLibrary(btLibrary);
			}

			return result;
		}

		/**
	 * Creates a BT library that contains all the behaviour trees in
	 * <code>behaviourTrees</code>. The name of the trees are specified in
	 * <code>names</code>, so, for instance, the i-th element in
	 * <code>names</code> represents the name of the i-th tree in
	 * <code>behaviourTrees</code>. If several trees are referenced by the same
	 * name, only the last one (according to its order in the input lists) will
	 * remain.
	 * 
	 * @param behaviourTrees
	 *            the list with the trees that the BT library will contain.
	 * @param names
	 *            the list with the names of the trees.
	 * @return a BT library that contains all the behaviour trees in the list
	 *         <code>behaviourTrees</code>.
	 */

		public static IBTLibrary createBTLibrary(List<ModelTask> behaviourTrees, List<string> names)
		{
			var result = new GenericBTLibrary();

			var treesIterator = behaviourTrees.GetEnumerator();
			var namesIterator = names.GetEnumerator();

			while (treesIterator.MoveNext() && namesIterator.MoveNext())
			{
				result.addBT(namesIterator.Current, treesIterator.Current);
			}

			return result;
		}
	}
}
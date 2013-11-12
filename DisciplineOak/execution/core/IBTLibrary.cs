/**
 * Common interface for all behaviour tree libraries. A behaviour tree library
 * is just a repository from which behaviour trees can be retrieved by name.
 * <p>
 * This is an <i>iterable</i> interface (it : {@link Iterable}) so that
 * all the behaviour trees of the library can be easily accessed.
 * 
 
 * 
 */

using DisciplineOak.Model.Core;

namespace DisciplineOak.Execution.Core
{
	public interface IBTLibrary
	{
		/**
	 * Returns the behaviour tree whose name is <code>name</code>. This method
	 * returns the root task of the tree.
	 * 
	 * @param name
	 *            the name of the tree to retrieve.
	 * @return the behaviour tree whose name is <code>name</code>, or null in
	 *         case it does not exist.
	 */
		ModelTask getBT(string name);
	}
}
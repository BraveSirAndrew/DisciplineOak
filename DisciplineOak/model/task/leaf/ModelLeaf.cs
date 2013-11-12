/**
 * Base class for all the tasks that have no children.
 * 
 
 * 
 */

using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Leaf
{
	public abstract class ModelLeaf : ModelTask
	{
		/**
		 * Constructs a ModelLeaf with a guard.
		 * 
		 * @param guard
		 *            the guard, which may be null.
		 */
		protected ModelLeaf(ModelTask guard) : base(guard)
		{
		}
	}
}
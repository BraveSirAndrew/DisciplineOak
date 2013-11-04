/**
 * Class representing an abstract basic action to be executed in the game. An
 * action is a task with no children (that is, it is a leaf in the behavior
 * tree) and with no connection to any other task in the tree.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using OhBehave.Model.Core;

namespace OhBehave.Model.Task.Leaf.action
{
	public abstract class ModelAction : ModelLeaf
	{
		/**
	 * Constructs the ModelAction.
	 * 
	 * @param guard
	 *            the guard of the ModelAction, which may be null.
	 */

		protected ModelAction(ModelTask guard) : base(guard)
		{
		}
	}
}
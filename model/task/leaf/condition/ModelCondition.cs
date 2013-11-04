/**
 * Class representing an abstract condition to be tested within the game.
 * Conditions are tasks with no children (that is, they are leaves in the
 * behavior tree) and with no connection to any other task in the tree.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Leaf.condition
{
	public abstract class ModelCondition : ModelLeaf
	{
		/**
	 * Constructs a ModelCondition.
	 * 
	 * @param guard
	 *            the guard of the ModelCondition, which may be null.
	 */

		protected ModelCondition(ModelTask guard) : base(guard)
		{
		}
	}
}
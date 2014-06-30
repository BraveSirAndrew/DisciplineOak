/**
 * Class representing an abstract condition to be tested within the game.
 * Conditions are tasks with no children (that is, they are leaves in the
 * behavior tree) and with no connection to any other task in the tree.
 * 
 
 * 
 */

using System;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Leaf.Condition
{
	[Serializable]
	public abstract class ModelCondition : ModelLeaf
	{
		protected ModelCondition(ModelTask guard) : base(guard)
		{
		}

		protected ModelCondition(ModelTask guard, string name)
			: base(guard, name)
		{
		}
	}
}
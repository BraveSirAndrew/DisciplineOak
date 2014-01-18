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
		protected ModelLeaf(ModelTask guard) : base(guard)
		{
		}

		protected ModelLeaf(ModelTask guard, string name) 
			: base(guard, name)
		{
		}
	}
}
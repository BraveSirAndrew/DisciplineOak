/**
 * Base class for all the tasks that have no children.
 * 
 
 * 
 */

using System;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Leaf
{
	[Serializable]
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
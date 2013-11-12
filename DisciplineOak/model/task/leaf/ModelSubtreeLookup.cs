/**
 * A ModelSubtreeLookup is a leaf node that emulates the behaviour of another
 * behaviour tree.
 * <p>
 * One of the key features of behaviour trees is that they can be reused in many
 * places. This reusability is implemented through the ModelSubtreeLookup task.
 * When a tree <i>A</i> must be reused within another tree <i>B</i>, this task
 * is used to retrieve <i>A</i> and use it within <i>B</i>. Trees are indexed by
 * names, so this task needs the name of the tree that it will emulate.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Leaf;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Leaf
{
	public class ModelSubtreeLookup : ModelLeaf
	{
		/** The name of the tree that this task is going to emulate. */
		private readonly string _treeName;

		/**
		 * Constructor.
		 * 
		 * @param guard
		 *            the guard of the task, which may be null.
		 * @param treeName
		 *            the name of the tree that this task is going to emulate.
		 */
		public ModelSubtreeLookup(ModelTask guard, string treeName)
			: base(guard)
		{
			this._treeName = treeName;
		}

		/**
		 * Returns the name of the tree that this task is going to emulate.
		 * 
		 * @return the name of the tree that this task is going to emulate.
		 */

		public string TreeName
		{
			get { return _treeName; }
		}

		/**
		 * Returns an ExecutionSubtreeLookup that is able to run this
		 * ModelSubtreeLookup.
		 */
		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionSubtreeLookup(this, executor, parent);
		}
	}
}
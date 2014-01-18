/**
 * A ModelComposite task is a task with several children, whose evaluation
 * depends on the evaluation of its children.
 * 
 
 * 
 */

using System;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Composite
{
	public abstract class ModelComposite : ModelTask
	{
		/**
	 * Constructor.
	 * <p>
	 * Constructs a ModelComposite with some children. A ModelComposite must
	 * have at least one child.
	 * 
	 * @param guard
	 *            the guard of the ModelComposite.
	 * @param children
	 *            the list of children. Must have at least one element.
	 */
		protected ModelComposite(ModelTask guard, params ModelTask[] children) : this(guard, null, children)
		{
		}

		protected ModelComposite(ModelTask guard, string name, params ModelTask[] children)
			: base(guard, name, children)
		{
			if (children.Length == 0)
			{
				throw new ArgumentException("The list of children cannot be empty");
			}
		}
	}
}
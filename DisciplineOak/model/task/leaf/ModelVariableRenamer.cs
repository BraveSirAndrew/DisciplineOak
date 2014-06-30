/**
 * A ModelVariableRenamer is a task that renames a variable of the context. This
 * task just takes one variable of the context and changes its name.
 * 
 
 * 
 */

using System;
using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Leaf;
using DisciplineOak.Model.Core;

namespace DisciplineOak.Model.Task.Leaf
{
	[Serializable]
	public class ModelVariableRenamer : ModelLeaf
	{
		/** The name of the variable that must be renamed. */
		/** The new name for the variable that must be renamed. */
		private readonly string _newVariableName;
		private readonly string _variableName;

		/**
		 * Constructor.
		 * 
		 * @param guard
		 *            the guard of the task, which may be null.
		 * @param variableName
		 *            the name of the variable to rename.
		 * @param newVariableName
		 *            the new name for the variable.
		 */
		public ModelVariableRenamer(ModelTask guard, string variableName, string newVariableName)
			: base(guard)
		{
			_variableName = variableName;
			_newVariableName = newVariableName;
		}


		/**
		 * Returns the name of the variable to rename.
		 * 
		 * @return the name of the variable to rename.
		 */

		public string VariableName
		{
			get { return _variableName; }
		}

		/**
		 * Returns the new name for the variable that must be renamed.
		 * 
		 * @return the new name for the variable that must be renamed.
		 */
		public string NewVariableName
		{
			get { return _newVariableName; }
		}

		/**
		 * Returns a new {@link ExecutionVariableRenamer} that knows how to run this
		 * ModelVariableRenamer.
		 * 
		 * @see jbt.model.core.ModelTask#createExecutor(jbt.execution.core.BTExecutor,
		 *      jbt.execution.core.ExecutionTask)
		 */

		public override ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent)
		{
			return new ExecutionVariableRenamer(this, executor, parent);
		}
	}
}
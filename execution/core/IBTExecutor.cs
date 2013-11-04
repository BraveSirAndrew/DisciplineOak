/**
 * A behaviour tree executor (entity that is able to run a behaviour tree) must
 * implement this interface.
 * <p>
 * This is the part of the behaviour trees execution interface that is exposed
 * to clients. The execution process of behaviour trees is driven by ticks,
 * which means that they only perform calculations when they are ticked. At
 * every tick, they are given a little amount of time to think and evolve as
 * expected. If a behaviour tree is not ticked, then it does not go on.
 * <p>
 * An IBTExecutor defines two main methods, one for ticking the behaviour tree
 * it is running, and another one for terminating the tree.
 * <p>
 * Behaviour trees are represented by the ModelTask class.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using OhBehave.Model.Core;

namespace OhBehave.Execution.Core
{
	public interface IBTExecutor
	{
		/**
	 * This method gives the underlying BT a little amount of time to run.
	 * <p>
	 * Initially, a IBTExecutor is created to run a particular BT (ModelTask).
	 * From then on, the <code>tick()</code> method is called to make the tree
	 * evolve.
	 * <p>
	 * Usually the AI of a game is driven by ticks, which means that
	 * periodically the AI is given some time to update its state (it checks the
	 * current game state and performs some actions). BTs follow this pattern,
	 * so whenever they are ticked, they are given a little amount of time to
	 * think and behave as expected. If BTs are not ticked, they do not consume
	 * CPU time and they not evolve.
	 * <p>
	 * By calling this method, the underlying BT will be ticked, so it will
	 * think and evolve accordingly.
	 * <p>
	 * Note that ticking a tree that has already finished should have no effect
	 * on the tree.
	 */
		void Tick();

		/**
	 * Terminates the execution of the behaviour tree. This method can be called
	 * even if the tree has not started running yet or if it has already been
	 * terminated.
	 */
		void Terminate();

		/**
	 * Returns the behaviour tree that this IBTExecutor is running. The
	 * behaviour tree is represented by its root, which is a single ModelTask
	 * object.
	 * 
	 * @return the behaviour tree that this IBTExecutor is running.
	 */
		ModelTask GetBehaviourTree();

		/**
	 * Returns the execution status of the behaviour tree. It is the status of
	 * the root of the tree.
	 * 
	 * @return the execution status of the behaviour tree. It is the status of
	 *         the root of the tree.
	 */
		Status GetStatus();

		/**
	 * Returns the context that was associated to the root node of the behaviour
	 * tree, and which is being used to run it.
	 * 
	 * @return the context that was associated to the root node of the behaviour
	 *         tree, and which is being used to run it.
	 */
		IContext GetRootContext();
	}
}
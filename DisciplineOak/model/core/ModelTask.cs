using System.Collections.Generic;
using DisciplineOak.Execution.Core;

namespace DisciplineOak.model.Core
{
	
	///ModelTask is a class that models a node (task) of a behaviour tree in a
	///conceptual way. A ModelTask does not have execution capabilities, since it
	///only purpose is to serve as a way of modeling a behaviour tree conceptually.
	///ModelTask is an abstract class, and it just acts as a container of other
	///child tasks, with maybe a guard.
	///
	///As stated above, a ModelTask cannot be run. The idea behind this model is
	///that an external interpreter should be in charge of running the behaviour
	///tree (ModelTask) instead of the task itself. By doing so, there is a clear
	///separation between both the conceptual and execution model, thus allowing to
	///have a unique model shared by many interpreters (which, in other words, means
	///that a single behaviour tree can be run by many entities at the same time).
	///
	///The interpreter that is used to run a behaviour tree -that is, a ModelTask
	///and all the tasks below it- is the BTExecutor class. The BTExecutor class is
	///used to run a ModelTask. A BTExecutor runs a behaviour tree by ticks. This
	///means that the tree is given some time to think and evolve only at certain
	///moments (ticks), and is doing nothing otherwise.
	///
	///Every ModelTask is able to issue an ExecutionTask capable of running it (
	///{@link #createExecutor(BTExecutor, ExecutionTask)}). Actually, the BTExecutor
	///uses ExecutionTask objects in order to run the conceptual behaviour tree. An
	///ExecutionTask is just another type of task that knows how to run its
	///corresponding ModelTask (by interacting with other tasks as well as with the
	///BTExecutor). For instance, a ModelSequence, which represents a sequence task
	///in a behaviour tree, has got an ExecutionTask that knows how to run it, the
	///ExecutionSequence. The <code>createExecutor()</code> method of ModelSequence
	///just returns an instance of ExecutionSequence. Therefore, the
	///<code>createExecutor()</code> method should just return an ExecutionTask that
	///knows how to run the ModelTask.
	///
	///@see ExecutionTask
	///@see BTExecutor
	public abstract class ModelTask
	{
		/** List of the children of the ModelTask. */
		private readonly List<ModelTask> _children;
		/**
		 * The guard of the ModelTask. It may be null, in which case it will always
		 * be evaluated to true.
		 */
		private readonly ModelTask _guard;
		/** The position of the ModelTask in the behaviour tree. */
		private Position _position;

		/**
		 * Creates a new ModelTask with a guard and several children. The guard may
		 * be null, in which case it is always evaluated to true. The task may also
		 * have no children.
		 * 
		 * @param guard
		 *            the guard, which may be null.
		 * @param children
		 *            the list of children.
		 */
		protected ModelTask(ModelTask guard, params ModelTask[] children)
		{
			_guard = guard;
			_children = new List<ModelTask>();

			foreach (var modelTask in children)
			{
				_children.Add(modelTask);
			}

			_position = new Position();
		}

		/**
		 * Returns the list of children of this task, or an empty list if it has no
		 * children. It should be noted that the children of a task are ordered, and
		 * the order influences the way the task runs, reason why a List is
		 * returned. Note that the returned list is the underlying list of children
		 * used by the task, so it should be used carefully (in general, it should
		 * never be modified).
		 * 
		 * @return the list of children of this task.
		 */
		public List<ModelTask> Children
		{
			get { return _children; }
		}

		/**
		 * Returns the guard of the task, which may be null.
		 * 
		 * @return the guard of the task, which may be null.
		 */

		public ModelTask Guard
		{
			get { return _guard; }
		}

		/**
		 * Returns the position that this task occupies in the behaviour tree. If it
		 * has not been computed yet (see {@link #computePositions()}), it returns a
		 * Position object with both x and y equal to -1.
		 * 
		 * @return the position that this task occupies in the behaviour tree.
		 */

		public Position Position
		{
			get { return _position; }
		}

		/**
		 * Creates a suitable ExecutionTask that will be able to run this ModelTask
		 * through the management of a BTExecutor.
		 * 
		 * @param executor
		 *            the BTExecutor that will manage the returned ExecutionTask.
		 * @param parent
		 *            the parent ExecutionTask for the returned ExecutionTask.
		 * 
		 * @return an ExecutionTask that is able to run this ModelTask.
		 */
		public abstract ExecutionTask CreateExecutor(BTExecutor executor, ExecutionTask parent);

		/**
		 * This method computes the positions of all the tasks of the behaviour tree
		 * whose root is this node. After calling this method, the positions of all
		 * the tasks below this one will be available and accessible through
		 * {@link #getPosition()}.
		 * <p>
		 * It is important to note that, when calling this method, this task is
		 * considered to be the root of the behaviour tree, so its position will be
		 * set to an empty sequence of moves, with no offset, and the positions of
		 * the tasks below it will be computed from it.
		 */
		public void ComputePositions()
		{
			/* Assume this node is the root of the tree. */
			_position = new Position(new List<int>());

			/*
			 * Set the position of all of the children of this task and recursively
			 * compute the position of the rest of the tasks.
			 */
			for (var i = 0; i < _children.Count; i++)
			{
				var currentChild = _children[i];
				var currentChildPos = new Position(_position);
				currentChildPos.AddMove(i);
				currentChild._position = currentChildPos;
				RecursiveComputePositions(currentChild);
			}
		}

		/**
		 * This function searches for a ModelTask according to a particular
		 * Position.
		 * <p>
		 * Conceptually, a Position represents a sequence of moves. This method just
		 * applies all moves in <code>moves</code> starting from this ModelTask, and
		 * returns the reached ModelTask, or null in case it does not exist.
		 * 
		 * @param moves
		 *            the sequence of moves that must be performed to retrieve the
		 *            ModelTask.
		 * @return the ModelTask obtained by moving down the tree according to the
		 *         sequence of moves <code>moves</code>, or null in case no
		 *         ModelTask could be found.
		 */
		public ModelTask FindNode(Position moves)
		{
			var m = moves.Moves;

			var currentTask = this;

			foreach (var currentMove in m)
			{
				var children = currentTask.Children;

				if (currentMove >= children.Count)
				{
					return null;
				}

				currentTask = children[currentMove];
			}

			return currentTask;
		}

		/**
		 * This method sets the positions of all tasks below <code>t</code> in the
		 * tree.
		 * 
		 * @param t
		 *            the task whose descendants will be computed their positions.
		 */
		private void RecursiveComputePositions(ModelTask t)
		{
			/*
			 * Set the position of all of the children of this task and recursively
			 * compute the position of the rest of the tasks.
			 */
			for (var i = 0; i < t._children.Count; i++)
			{
				var currentChild = t._children[i];
				var currentChildPos = new Position(t.Position);
				currentChildPos.AddMove(i);
				currentChild._position = currentChildPos;
				RecursiveComputePositions(currentChild);
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace OhBehave.Model.Core
{
	/**
	 * The position of a ModelTask in a behaviour tree. It contains the sequence
	 * of moves that must be performed to go from the root to the node itself.
	 * Each of the moves of the sequence represents what child of the current
	 * node must be selected. For instance, if the position represents the
	 * sequence of moves {1,4,0}, the node that it points to is the first child
	 * (0) of the fifth child (4) of the second child (1) of the root. En empty
	 * list of moves represents the root.
	 * 
	 * @author Ricardo Juan Palma Durán
	 * 
	 */
	public class Position
	{
		/**
		 * The list of moves that this position represents.
		 */
		private readonly List<int> _moves;

		/**
		 * Constructs an Position that contains the moves specified in its
		 * constructor, in the same order. If no move is specified, the Position
		 * will represent an empty sequence of moves.
		 */
		public Position(params int[] moves)
		{
			_moves = new List<int>();
			_moves.AddRange(moves);
		}

		/**
		 * Constructs a Position from a sequence of moves represented as a List.
		 * 
		 * @param moves
		 *            the sequence of moves that this Position will represent.
		 */
		public Position(IEnumerable<int> moves)
		{
			if (moves == null)
			{
				throw new Exception("The list of moves cannot be null");
			}

			_moves = new List<int>();
			_moves.AddRange(moves);
		}

		/**
		 * Constructs a copy of the Position <code>pos</code>.
		 * 
		 * @param pos
		 *            the Position that is copied.
		 */
		public Position(Position pos)
		{
			_moves = new List<int>();

			foreach (var move in pos._moves)
			{
				_moves.Add(move);
			}
		}

		/**
		 * Returns the sequence of moves that this Position represents.
		 * 
		 * @return the sequence of moves that this Position represents.
		 */

		public List<int> Moves
		{
			get { return new List<int>(_moves); }
		}

		/**
		 * Adds a move to this Position. The move is inserted as the last one of
		 * the sequence.
		 * 
		 * @param move
		 *            the move to add.
		 * @return this Position.
		 */
		public Position AddMove(int move)
		{
			_moves.Add(move);
			return this;
		}

		/**
		 * Adds a list of moves to this Position. The moves are inserted in the
		 * order specified in the <code>moves</code> list.
		 * 
		 * @param moves
		 *            the list of moves to add.
		 * @return this Position.
		 */
		public Position AddMoves(IEnumerable<int> moves)
		{
			_moves.AddRange(moves);
			return this;
		}

		/**
		 * Adds the moves of a Position to this Position.
		 * 
		 * @param position
		 *            the position whose moves are going to be added to this
		 *            one.
		 * @return this Position.
		 */
		public Position AddMoves(Position position)
		{
			AddMoves(position.Moves);
			return this;
		}

		public override string ToString()
		{
			var result = "";
			if (_moves.Count != 0)
			{
				for (int i = 0; i < _moves.Count; i++)
				{
					result += i + " ";
				}

				return "[" + result.Substring(0, result.Length - 1) + "]";
			}
			return "[]";
		}

		/**
		 * Returns true if <code>o</code> is a Position object that contains the
		 * same sequence of moves as that of this. Returns false otherwise.
		 * 
		 * // /**
		// * Compares this Position object to another one. Let <i>A</i> and
		// * <i>B</i> be two Position objects. If <i>A</i> is at a higher
		// level
		// in
		// * the tree, then <i>A</i> is less than <i>B</i>. If <i>A</i> is at
		// a
		// * lower lever in the tree, then <i>A</i> is greater than <i>B</i>.
		// If
		// * <i>A</i> is at the same level in the tree as that of <i>B</i>,
		// then:
		// * <ul>
		// * <li>If <i>A</i> is at the left of <i>B</i>, then <i>A</i> is less
		// * than <i>B</i>.
		// * <li>If <i>A</i> represents the same sequence of moves as that of
		// * <i>B</i>, then <i>A</i> equals <i>B</i>.
		// * <li>Otherwise, <i>A</i> is greater than <i>B</i>.
		// * </ul>
		// *
		 * 
		 */
		public override bool Equals(Object o)
		{
			if (this == o)
			{
				return true;
			}

			if (!(o is Position))
			{
				return false;
			}

			var oPosition = (Position) o;

			var thisMoves = Moves;
			var oMoves = oPosition.Moves;

			if (oMoves.Count != thisMoves.Count)
			{
				return false;
			}

			if (thisMoves.SequenceEqual(oMoves))
				return true;

			return false;
		}

		public override int GetHashCode()
		{
			return _moves.GetHashCode();
		}
	}
}
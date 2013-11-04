/**
 * Exception thrown when there is an error in the ticking process of a task.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;

namespace OhBehave.Exceptions
{
	public abstract class TickException : Exception
	{
		protected TickException()
		{
		}

		protected TickException(string msg) : base(msg)
		{
		}
	}
}
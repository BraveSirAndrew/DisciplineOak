/**
 * Exception thrown when there is an error in the ticking process of a task.
 * 
 * 
 */

using System;

namespace DisciplineOak.Exceptions
{
	public abstract class TickException : Exception
	{
		protected TickException(string msg) : base(msg)
		{
		}
	}
}
/**
 * Exception thrown when a task that cannot be ticked is ticked.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using OhBehave.Exceptions;

namespace OhBehave.exception
{
	public class NotTickableException : TickException
	{
		public NotTickableException(string msg) : base(msg)
		{
		}
	}
}
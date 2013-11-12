/**
 * Exception thrown when a task that cannot be ticked is ticked.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;

namespace DisciplineOak.exception
{
	public class NotTickableException : TickException
	{
		public NotTickableException(string msg) : base(msg)
		{
		}
	}
}
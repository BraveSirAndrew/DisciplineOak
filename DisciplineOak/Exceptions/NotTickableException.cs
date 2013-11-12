/**
 * Exception thrown when a task that cannot be ticked is ticked.
 * 
 
 * 
 */

namespace DisciplineOak.Exceptions
{
	public class NotTickableException : TickException
	{
		public NotTickableException(string msg) : base(msg)
		{
		}
	}
}
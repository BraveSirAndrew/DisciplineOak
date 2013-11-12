/**
 * Exception thrown when there is an error in the ticking process of a task.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

namespace DisciplineOak.Exception
{
	public abstract class TickException : System.Exception
	{
		protected TickException()
		{
		}

		protected TickException(string msg) : base(msg)
		{
		}
	}
}
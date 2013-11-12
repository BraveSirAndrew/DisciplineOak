/**
 * Exception thrown when {@link ExecutionTask#internalTick()} returns a Status
 * that is not allowed.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

namespace DisciplineOak.exception
{
	public class IllegalReturnStatusException : TickException
	{
		public IllegalReturnStatusException()
		{
		}

		public IllegalReturnStatusException(string msg) : base(msg)
		{
		}
	}
}
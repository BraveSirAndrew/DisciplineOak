namespace DisciplineOak.Exceptions
{
	/// <summary>
	///  Exception thrown when {@link ExecutionTask#internalTick()} returns a Status that is not allowed.
	/// </summary>
	public class IllegalReturnStatusException : TickException
	{
		public IllegalReturnStatusException(string msg) : base(msg)
		{
		}
	}
}
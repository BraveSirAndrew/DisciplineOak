/**
 * Exception thrown when a task that cannot be spawned is spawned.
 * 
 
 * 
 */

namespace DisciplineOak.Exceptions
{
	public class SpawnException : System.Exception
	{
		public SpawnException(string msg) : base(msg)
		{
		}
	}
}
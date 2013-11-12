/**
 * Exception thrown when a task that cannot be spawned is spawned.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

namespace DisciplineOak.Exception
{
	public class SpawnException : System.Exception
	{
		public SpawnException()
		{
		}

		public SpawnException(string msg) : base(msg)
		{
		}
	}
}
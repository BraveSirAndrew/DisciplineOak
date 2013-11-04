/**
 * Exception thrown when a task that cannot be spawned is spawned.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;

namespace OhBehave.exception
{
	public class SpawnException : Exception
	{
		public SpawnException()
		{
		}

		public SpawnException(string msg) : base(msg)
		{
		}
	}
}
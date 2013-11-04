/**
 * Exception thrown when {@link ExecutionTask#internalTick()} returns a Status
 * that is not allowed.
 * 
 * @author Ricardo Juan Palma Durán
 * 
 */

using System;
using OhBehave.Exceptions;

namespace OhBehave.exception
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
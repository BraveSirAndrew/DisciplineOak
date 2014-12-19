using DisciplineOak.Execution.Core;
using DisciplineOak.Execution.Task.Decorator;
using DisciplineOak.Model.Task.Decorator;
using DisciplineOak.Model.Task.Leaf;
using Moq;
using NUnit.Framework;

namespace DisciplineOak.Tests.Execution.Core
{
	[TestFixture]
	public class BTExecutorTests
	{
		[TestFixture]
		public class Interrupters
		{
			[Test]
			public void CanRegisterAnInterrupter()
			{
				var executor = new BTExecutor(new ModelSuccess(null));
				var modelInterrupter = new ModelInterrupter(null, new ModelSuccess(null));

				executor.RegisterInterrupter((ExecutionInterrupter)modelInterrupter.CreateExecutor(executor, null));

				Assert.NotNull(executor.GetExecutionInterrupter(modelInterrupter));
			}

			[Test]
			public void CanUnRegisterAnInterrupter()
			{
				var executor = new BTExecutor(new ModelSuccess(null));
				var modelInterrupter = new ModelInterrupter(null, new ModelSuccess(null));

				var executionInterrupter = (ExecutionInterrupter) modelInterrupter.CreateExecutor(executor, null);
				executor.RegisterInterrupter(executionInterrupter);
				executor.UnregisterInterrupter(executionInterrupter);

				Assert.Null(executor.GetExecutionInterrupter(modelInterrupter));
			}
		}
	}
}

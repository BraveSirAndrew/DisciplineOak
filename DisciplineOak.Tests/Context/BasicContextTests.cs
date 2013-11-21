using DisciplineOak.Execution.Context;
using NUnit.Framework;

namespace DisciplineOak.Tests.Context
{
	[TestFixture]
	public class BasicContextTests
	{
		[Test]
		public void Indexer_doesnt_throw_when_passed_unknown_variable_name()
		{
			var context = new BasicContext();
			Assert.DoesNotThrow(() =>
			{
				var str = (string) context["unknown"];
			});
		}
	}
}

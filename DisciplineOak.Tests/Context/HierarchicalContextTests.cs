using DisciplineOak.Execution.Context;
using DisciplineOak.Execution.Core;
using NUnit.Framework;

namespace DisciplineOak.Tests.Context
{
	[TestFixture]
	public class HierarchicalContextTests
	{
		[Test]
		public void Indexer_pulls_variables_from_parent_when_they_dont_exist_in_the_current_context()
		{
			var parentContext = new HierarchicalContext();
			parentContext.SetVariable("Hello", true);

			var childContext = new HierarchicalContext();
			childContext.SetParent(parentContext);

			var context = (IContext) childContext;

			Assert.IsTrue((bool) context["Hello"]);
		}
	}
}

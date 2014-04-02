using DisciplineOak.Execution.Context;
using DisciplineOak.Model.Core;
using NUnit.Framework;

namespace DisciplineOak.Tests.Context
{
	[TestFixture]
	public class GenericBTLibraryTests
	{
		private const string _firstTree = "FirstTree";
		private const string _secondTree = "SecondTree";
		private TestTask _tree;
		private TestTask _tree2;
		private GenericBTLibrary _library;

		[SetUp]
		public void SetUp()
		{
			_library = new GenericBTLibrary();

			_tree = new TestTask(null, "tree1", new ModelTask[0]);
			_tree2 = new TestTask(null, "tree2", new ModelTask[0]);
		}

		[Test]
		public void When_tree_added_can_be_retrieved()
		{
			_library.AddBT(_firstTree, _tree);
			_library.AddBT(_secondTree, _tree2);

			Assert.AreEqual(_tree.Name, _library.GetBT(_firstTree).Name);
		}

		[Test]
		public void When_null_tree_added_does_not_throw()
		{
			Assert.DoesNotThrow(() => _library.AddBT(_firstTree, null));
		}

		[Test]
		public void When_null_tree_added_overwrites_existing()
		{
			_library.AddBT(_firstTree, _tree);
			_library.AddBT(_firstTree, null);
			
			Assert.AreEqual(null, _library.GetBT(_firstTree));
		}

		[Test]
		public void When_existing_tree_added_previous_tree_overwritten()
		{
			_library.AddBT(_firstTree, _tree);
			_library.AddBT(_firstTree, _tree2);

			Assert.AreEqual(_tree2.Name, _library.GetBT(_firstTree).Name);
			Assert.AreNotEqual(_tree.Name, _library.GetBT(_firstTree).Name);
		}

		[Test]
		public void When_library_added_all_trees_added()
		{
			var library = new GenericBTLibrary();
			library.AddBT(_firstTree, _tree);
			library.AddBT(_secondTree, _tree2);

			_library.AddBTLibrary(library);

			Assert.AreEqual(_tree.Name, _library.GetBT(_firstTree).Name);
			Assert.AreEqual(_tree2.Name, _library.GetBT(_secondTree).Name);
		}

		[Test]
		public void When_null_library_added_does_not_throw()
		{
			Assert.DoesNotThrow(() => _library.AddBTLibrary(null));
		}

		[Test]
		public void When_library_added_with_existing_trees_then_previous_trees_overwritten()
		{
			const string libraryTreeName = "libraryTree";

			_library.AddBT(_firstTree, _tree);

			var library = new GenericBTLibrary();
			library.AddBT(_firstTree, new TestTask(null, libraryTreeName, new ModelTask[0]));
			library.AddBT(_secondTree, _tree2);

			_library.AddBTLibrary(library);

			Assert.AreEqual(libraryTreeName, _library.GetBT(_firstTree).Name);
			Assert.AreNotEqual(_tree.Name, _library.GetBT(_firstTree).Name);
			Assert.AreEqual(_tree2.Name, _library.GetBT(_secondTree).Name);
		}
	}
}

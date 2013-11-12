namespace DisciplineOak.Model.Task.composite
{
	public enum ParallelPolicy
	{
		/**
		 * Policy meaning that the parallel behaves like a sequence task, that
		 * is, it fails as soon as one of its children fail, and it only succeed
		 * if all of its children succeed.
		 */
		SequencePolicy,
		/**
		 * Policy meaning the parallel behaves like a selector task, that is, if
		 * succeeds as soon as one of its children succeed, and it only fails of
		 * all of its children fail.
		 */
		SelectorPolicy
	}
}
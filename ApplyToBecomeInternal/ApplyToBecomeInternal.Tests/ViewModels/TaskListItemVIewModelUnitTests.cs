using ApplyToBecomeInternal.ViewModels;
using Xunit;

namespace ApplyToBecomeInternal.Tests.ViewModels
{
	public class TaskListItemVIewModelUnitTests
	{
		[Fact]
		public void Should_return_true_when_equivalent()
		{
			Assert.Multiple(
				() => Assert.True(TaskListItemViewModel.NotStarted.Equals((object) TaskListItemViewModel.NotStarted)),
				() => Assert.True(TaskListItemViewModel.InProgress.Equals((object)TaskListItemViewModel.InProgress)),
				() => Assert.True(TaskListItemViewModel.Completed.Equals((object)TaskListItemViewModel.Completed))
			);
		}

		[Fact]
		public void Should_return_false_when_not_equivalent()
		{
			Assert.Multiple(
				() => Assert.False(TaskListItemViewModel.NotStarted.Equals((object)TaskListItemViewModel.Completed)),
				() => Assert.False(TaskListItemViewModel.InProgress.Equals(null))
			);
		}
	}
}

using Dfe.PrepareConversions.ViewModels;
using Xunit;

namespace Dfe.PrepareConversions.Tests.ViewModels
{
	public class TaskListItemViewModelUnitTests
	{
		[Fact]
		public void Should_return_true_when_equivalent()
		{
			Assert.Multiple(
				() => Assert.True(TaskListItemViewModel.NotStarted.Equals((object)TaskListItemViewModel.NotStarted)),
				() => Assert.True(TaskListItemViewModel.InProgress.Equals((object)TaskListItemViewModel.InProgress)),
				() => Assert.True(TaskListItemViewModel.Completed.Equals((object)TaskListItemViewModel.Completed))
			);
		}

		[Fact]
		public void Should_return_false_when_not_equivalent()
		{
			Assert.False(TaskListItemViewModel.NotStarted.Equals((object)TaskListItemViewModel.Completed));
		}

		[Fact]
		public void Should_return_false_when_not_null()
		{
			Assert.False(TaskListItemViewModel.InProgress.Equals(null));
		}

		[Fact]
		public void Should_return_true_when_equal()
		{
			var model = TaskListItemViewModel.NotStarted;

			Assert.True(model.Equals(model));
		}

		[Fact]
		public void GetHashCode_should_return_the_same_hashcode_when_objects_are_equivalent()
		{
			Assert.Equal(TaskListItemViewModel.NotStarted.GetHashCode(), TaskListItemViewModel.NotStarted.GetHashCode());
		}
	}
}

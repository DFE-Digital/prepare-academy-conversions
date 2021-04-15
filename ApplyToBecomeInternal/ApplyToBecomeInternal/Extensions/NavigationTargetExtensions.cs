using ApplyToBecomeInternal.Models.Shared;

namespace ApplyToBecomeInternal.Extensions
{
	public static class NavigationTargetExtensions
	{
		public static (string Content, string Url) GetContent(this NavigationTarget target)
		{
			var navigationAttribute = target.GetAttribute<NavigationAttribute>();
			return (navigationAttribute.Content, navigationAttribute.Url);
		}
	}
}
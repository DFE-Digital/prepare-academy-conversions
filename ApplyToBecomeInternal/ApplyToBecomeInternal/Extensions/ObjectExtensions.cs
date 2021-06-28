namespace ApplyToBecomeInternal.Extensions
{
	public static class ObjectExtensions
	{
		public static string ToStringOrDefault(this object obj, string @default = null)
		{
			if (obj == null)
			{
				return @default;
			}
			return obj.ToString();
		}
	}
}
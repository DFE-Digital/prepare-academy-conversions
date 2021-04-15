using System;
using System.Linq;
using System.Reflection;

namespace ApplyToBecomeInternal.Extensions
{
	public static class ObjectExtensions
	{
		public static TAttribute GetAttribute<TAttribute>(this object value) where TAttribute : Attribute =>
			value.GetType().GetMember(value.ToString()).First().GetCustomAttribute<TAttribute>();
	}
}

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ApplyToBecomeInternal.Utils
{
	public class Typespace
	{
		private static readonly Regex NonAlphaNumeric =
			new Regex("[^a-z0-9-]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private Typespace() { }

		public static Typespace Name => new Typespace();

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static implicit operator string(Typespace _)
		{
			StackFrame frame = new StackFrame(1);
			MethodBase method = frame.GetMethod();
			Type declaringType = method?.DeclaringType;

			if (declaringType is { Namespace: { } })
			{
				ReadOnlySpan<char> typeName = declaringType.FullName.AsSpan(declaringType.Namespace.Length + 1);
				ReadOnlySpan<char> memberName = RemovePropertyPrefixes(method?.Name);

				return NonAlphaNumeric.Replace($"{typeName.ToString()}-{memberName.ToString()}", "-").ToLowerInvariant();
			}

			return string.Empty;
		}

		private static ReadOnlySpan<char> RemovePropertyPrefixes(string? name)
		{
			if (string.IsNullOrWhiteSpace(name)) return ReadOnlySpan<char>.Empty;

			ReadOnlySpan<char> nameSpan = name.AsSpan();

			return nameSpan.StartsWith("get_", StringComparison.InvariantCultureIgnoreCase) ||
			       nameSpan.StartsWith("set_", StringComparison.InvariantCultureIgnoreCase)
				? nameSpan[4..]
				: nameSpan;
		}
	}
}

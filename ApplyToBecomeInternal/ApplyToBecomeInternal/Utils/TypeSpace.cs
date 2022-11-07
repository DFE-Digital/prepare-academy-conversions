using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ApplyToBecomeInternal.Utils
{
	public class Typespace
	{
		private static readonly Regex NonAlphaNumeric =
			new Regex("[^a-z0-9-]", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private readonly string _suffix;

		private Typespace(string suffix)
		{
			_suffix = suffix;
		}

		/// <summary>
		///     Enables or disables the Typespace functionality
		/// </summary>
		public static bool IsEnabled { get; set; } = true;

		/// <summary>
		///     Generates the textual representation of the nested type hierarchy
		/// </summary>
		/// <param name="cachedValue">parameter in to which to cache the generated value</param>
		/// <param name="disabledDefault">the value to return when Typespace is disabled. (default: string.Empty)</param>
		/// <returns>string representing the type hierarchy if enabled, otherwise <paramref name="disabledDefault" /> is returned</returns>
		public static string Name(ref string cachedValue, string disabledDefault = "")
		{
			cachedValue ??= new Typespace(null);

			return IsEnabled
				? cachedValue
				: disabledDefault;
		}

		/// <summary>
		///     Generates the textual representation of the nested type hierarchy
		/// </summary>
		/// <param name="suffix">contextual suffix to be added to the end of the generated output</param>
		/// <param name="disabledDefault">the value to return when Typespace is disabled. (default: string.Empty)</param>
		/// <returns>string representing the type hierarchy if enabled, otherwise <paramref name="disabledDefault" /> is returned</returns>
		/// <remarks>This version does not support caching due to variation by suffix</remarks>
		public static string Name(string suffix, string disabledDefault = "")
		{
			return IsEnabled
				? new Typespace(suffix)
				: disabledDefault;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static implicit operator string(Typespace instance)
		{
			StackFrame frame = new StackFrame(2);
			MethodBase method = frame.GetMethod();
			Type declaringType = method?.DeclaringType;

			if (declaringType is { Namespace: { } })
			{
				ReadOnlySpan<char> typeName = declaringType.FullName.AsSpan(declaringType.Namespace.Length + 1);
				ReadOnlySpan<char> memberName = RemovePropertyPrefixes(method.Name);

				string[] components = { typeName.ToString(), memberName.ToString(), instance._suffix };
				return NonAlphaNumeric.Replace(string.Join('-', components.Where(x => string.IsNullOrWhiteSpace(x) is false)), "-").ToLowerInvariant();
			}

			return string.Empty;
		}

		private static ReadOnlySpan<char> RemovePropertyPrefixes(string name)
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

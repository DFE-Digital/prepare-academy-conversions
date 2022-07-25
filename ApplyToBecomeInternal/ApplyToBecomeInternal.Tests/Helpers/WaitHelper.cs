using System;

namespace ApplyToBecomeInternal.Tests.Helpers
{
	public class WaitHelper
	{
		public static void WaitUntil(Func<bool> condition, int timeoutMs = 2000)
		{
			System.Threading.SpinWait.SpinUntil(condition, timeoutMs);

			if (!condition())
			{
				throw new TimeoutException($"Condition was not satisfied after {timeoutMs} milliseconds.");
			}
		}
	}
}
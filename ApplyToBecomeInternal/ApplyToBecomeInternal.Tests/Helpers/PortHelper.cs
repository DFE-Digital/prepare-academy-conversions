namespace ApplyToBecomeInternal.Tests.Helpers
{
	public static class PortHelper
	{
		private static int _currentPort = 5080;
		private static object _sync = new object();

		public static int AllocateNext()
		{
			lock (_sync)
			{
				var next = _currentPort;
				_currentPort++;
				return next;
			}
		}
	}
}

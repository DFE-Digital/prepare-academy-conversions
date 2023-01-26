namespace Dfe.PrepareConversions.Data.Services
{
	public sealed class ApiV2Wrapper<T>
	{
		public T Data { get; set; }
		public ApiV2PagingInfo Paging { get; set; }
	}
}

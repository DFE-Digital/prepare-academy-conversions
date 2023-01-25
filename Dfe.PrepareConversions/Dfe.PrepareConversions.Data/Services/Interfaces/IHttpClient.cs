using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Interfaces
{
	public interface IConversionsHttpClient
	{
		public Task<HttpResponseMessage> GetAsync(string url);

		public Task<HttpResponseMessage> PostAsync(string url, HttpContent content);

		public Task<HttpResponseMessage> PatchAsync(string url, HttpContent content);
	}
}

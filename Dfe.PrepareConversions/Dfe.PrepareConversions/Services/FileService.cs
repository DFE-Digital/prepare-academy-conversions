using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dfe.PrepareConversions.Services.Helpers;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dfe.PrepareConversions.Services;

public interface IFileService
{
   Task<List<string>> GetFiles(string entityName, string recordId, string recordName, string fieldName);
   Task<string> DownloadFile(string entityName, string recordId, string recordName, string fieldName, string fileName);
}

public class FileService : IFileService
{
   private readonly HttpClient _httpClient;
   private readonly IAadAuthorisationHelper _aadAuthorisationHelper;

   public FileService() { }

   public FileService(HttpClient httpClient, IAadAuthorisationHelper aadAuthorisationHelper)
   {
      _httpClient = httpClient;
      _aadAuthorisationHelper = aadAuthorisationHelper;
   }

   public async Task<List<string>> GetFiles(string entityName, string recordId, string recordName, string fieldName)
   {
      var url = $"?entityName={entityName}&recordName={recordName}&recordId={recordId}&fieldName={fieldName}";

      using var request = new HttpRequestMessage(HttpMethod.Get, url);

      var content = await DoHttpRequest(request);

      return ParseJResponse(content);
   }

   public async Task<string> DownloadFile(string entityName, string recordId, string recordName, string fieldName, string fileName)
   {
      var url = $@"download?entityName={entityName}&recordName={recordName}&recordId={recordId}&fieldName={fieldName}&fileName={fileName}";

      using var request = new HttpRequestMessage(HttpMethod.Get, url);
      var content = await DoHttpRequest(request);

      return content;

   }

   private async Task<string> DoHttpRequest(HttpRequestMessage request)
   {
      var accessToken = await _aadAuthorisationHelper.GetAccessToken();
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

      var response = await _httpClient.SendAsync(request);
      var content = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
         throw new Exception($"The file service failed with a status of {response.ReasonPhrase} {content}");

      var receiveStream = await response.Content.ReadAsStreamAsync();
      using var readStream = new StreamReader(receiveStream, Encoding.UTF8);
      return await readStream.ReadToEndAsync();
   }

   private List<string> ParseJResponse(string content)
   {
      var jobject = JObject.Parse(content);
      var jfiles = (JArray)jobject?.GetValue("Files", StringComparison.OrdinalIgnoreCase)!;
      return jfiles.Select(x => (string)x).ToList();
   }

   internal static string GetJsonAsString(JObject jObject)
   {
      var sb = new StringBuilder();
      sb.Append("\"");
      using (var sw = new StringWriter(sb))
      using (var writer = new JsonTextWriter(sw))
      {
         writer.QuoteChar = '\'';

         var ser = new JsonSerializer();
         ser.Serialize(writer, jObject);
      }

      sb.Append("\"");

      return sb.ToString();
   }
}

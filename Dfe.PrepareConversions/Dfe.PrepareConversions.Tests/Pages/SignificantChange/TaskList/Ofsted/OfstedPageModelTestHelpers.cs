using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Pages.SignificantChange.TaskList.Ofsted;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Tests.Pages.SignificantChange.TaskList.Ofsted;

internal static class OfstedPageModelTestHelpers
{
   public static SignificantChangeProjectResponse BuildProject(int id)
   {
      return new SignificantChangeProjectResponse
      {
         Id = id,
         Urn = 10000000 + id,
         SchoolName = "Test school",
         Tier = 1,
         TrustName = "Example trust",
         TrustUkprn = "12345678",
         TypeOfSignificantChange = "Route A",
         ApplicationId = "ID_APP_123",
         ApplicationReference = "APP_REF_123",
         Status = "pre decision",
         LocalAuthorityName = "Test local authority"
      };
   }

   public static Mock<ISignificantChangeProjectRepository> BuildRepository(
      int id,
      SignificantChangeProjectResponse project,
      HttpStatusCode statusCode = HttpStatusCode.OK)
   {
      Mock<ISignificantChangeProjectRepository> repository = new();
      repository
         .Setup(x => x.GetProjectById(id))
         .ReturnsAsync(new ApiResponse<SignificantChangeProjectResponse>(statusCode, project));

      return repository;
   }

   public static void SetInspection(
      MemorySession session,
      int id,
      SetSignificantChangeOfstedInspectionCommand inspection)
   {
      session.SetString($"{OfstedBaseModel.OFSTED_INSPECTION_SESSION_KEY}_{id}", JsonSerializer.Serialize(inspection));
   }

   public static SetSignificantChangeOfstedInspectionCommand GetInspection(MemorySession session, int id)
   {
      string value = session.GetString($"{OfstedBaseModel.OFSTED_INSPECTION_SESSION_KEY}_{id}");
      return value is null ? null : JsonSerializer.Deserialize<SetSignificantChangeOfstedInspectionCommand>(value);
   }

   public static void SetPageContext(PageModel model)
   {
      DefaultHttpContext httpContext = new();
      ModelStateDictionary modelState = new();
      ActionContext actionContext = new(httpContext, new RouteData(), new ActionDescriptor(), modelState);
      model.PageContext = new PageContext(actionContext)
      {
         ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), modelState)
      };
   }

   internal sealed class MemorySession : ISession
   {
      private readonly Dictionary<string, byte[]> _values = new();

      public bool IsAvailable => true;
      public string Id => "test-session";
      public IEnumerable<string> Keys => _values.Keys;

      public void Clear() => _values.Clear();

      public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

      public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

      public void Remove(string key) => _values.Remove(key);

      public void Set(string key, byte[] value) => _values[key] = value;

      public bool TryGetValue(string key, out byte[] value) => _values.TryGetValue(key, out value);
   }
}

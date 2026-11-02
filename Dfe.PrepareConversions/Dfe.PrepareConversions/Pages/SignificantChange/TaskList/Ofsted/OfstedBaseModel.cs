using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Microsoft.AspNetCore.Http;
using System;

namespace Dfe.PrepareConversions.Pages.SignificantChange.TaskList.Ofsted
{
   public abstract class OfstedBaseModel(ISignificantChangeProjectRepository repository, ISession session) : BaseSignificantChangeTaskPageModel(repository)
   {
      public const string OFSTED_INSPECTION_SESSION_KEY = "SignificantChangeOfstedInspection";
      internal static string OfstedSessionKey(int id) => $"{OFSTED_INSPECTION_SESSION_KEY}_{id}";

      protected SetSignificantChangeOfstedInspectionCommand GetOfstedInspectionFromSession(int id)
      {
         return session.Get<SetSignificantChangeOfstedInspectionCommand>(OfstedSessionKey(id));
      }

      protected void SetOfstedInspectionInSession(int id, SetSignificantChangeOfstedInspectionCommand inspection)
      {
         session.Set(OfstedSessionKey(id), inspection);
      }

      public string GetTaskTitleQuestion(int id)
      {
         var inspection = GetOfstedInspectionFromSession(id);

         var title = inspection?.InspectionDate switch
         {
            { } date when date < new DateTime(2024, 9, 19, 0, 0, 0, DateTimeKind.Utc) => "Is the academy rated good or outstanding?",
            { } date when date <= new DateTime(2025, 11, 9, 0, 0, 0, DateTimeKind.Utc) =>
               "Is the academy rated good or outstanding in leadership and management and quality of education?",
            _ => "Is the academy meeting expected standard, strong standard or exceptional in all evaluation areas?"
         };

         return title;
      }
   }
}

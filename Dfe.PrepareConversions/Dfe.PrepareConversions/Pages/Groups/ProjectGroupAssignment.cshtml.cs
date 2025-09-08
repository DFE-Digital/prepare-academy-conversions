using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Dfe.PrepareConversions.Models.Links;

namespace Dfe.PrepareConversions.Pages.Groups;

public class ProjectGroupAssignmentModel(IUserRepository userRepository, IProjectGroupsRepository projectGroupsRepository) : PageModel
{
   public int Id { get; set; }
   public IEnumerable<User> DeliveryOfficers { get; set; }
   public string SelectedDeliveryOfficer { get; set; }
   public string GroupName { get; set; }

   public async Task<IActionResult> OnGet(int id)
   {
      var projectResponse = await projectGroupsRepository.GetProjectGroupById(id);
      Id = id;
      GroupName = $"{projectResponse.Body.TrustName} - {projectResponse.Body.ReferenceNumber}";
      SelectedDeliveryOfficer = projectResponse.Body?.AssignedUser?.FullName;

      DeliveryOfficers = await userRepository.GetAllUsers();

      return Page();
   }

   public async Task<IActionResult> OnPost(int id, string selectedName, bool unassignDeliveryOfficer, string deliveryOfficerInput)
   {
      var projectResponse = await projectGroupsRepository.GetProjectGroupById(id);

      if (unassignDeliveryOfficer)
      {
         await projectGroupsRepository.AssignProjectGroupUser(projectResponse.Body.ReferenceNumber, new SetAssignedUserModel(id, Guid.Empty, string.Empty, string.Empty));
         TempData.SetNotification(NotificationType.Success, "Done", "Project is unassigned");
      }
      else if (!string.IsNullOrEmpty(selectedName))
      {
         var deliveryOfficers = await userRepository.GetAllUsers();

         var assignedUser = deliveryOfficers.SingleOrDefault(u => u.FullName == selectedName);

         await projectGroupsRepository.AssignProjectGroupUser(projectResponse.Body.ReferenceNumber, new SetAssignedUserModel(id, new Guid(assignedUser.Id), assignedUser.FullName, assignedUser.EmailAddress));
         TempData.SetNotification(NotificationType.Success, "Done", "Project is assigned");
      }

      (string returnPage, string fragment) = GetReturnPageAndFragment();

      if (!string.IsNullOrWhiteSpace(returnPage))
      {
         return RedirectToPage(returnPage, null, new { id }, fragment);
      }
      else
      {
         return RedirectToPage(ProjectGroups.ProjectGroupIndex.Page, new { projectResponse.Body.Id, isNew = false });
      }
   }

   private (string, string) GetReturnPageAndFragment()
   {
      Request.Query.TryGetValue("return", out StringValues returnQuery);
      Request.Query.TryGetValue("fragment", out StringValues fragmentQuery);
      return (returnQuery, fragmentQuery);
   }
}

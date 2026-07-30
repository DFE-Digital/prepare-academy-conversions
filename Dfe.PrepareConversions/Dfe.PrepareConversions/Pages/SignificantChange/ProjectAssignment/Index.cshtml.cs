using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Models.SignificantChange;
using Dfe.PrepareConversions.Data.Services;
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

namespace Dfe.PrepareConversions.Pages.SignificantChange.ProjectAssignment;

public class IndexModel(IUserRepository userRepository, ISignificantChangeProjectRepository _significantChangeProjectRepository) : PageModel
{
   public string SchoolName { get; private set; }
   public int Id { get; set; }
   public IEnumerable<User> DeliveryOfficers { get; set; }
   public string SelectedDeliveryOfficer { get; set; }

   public async Task<IActionResult> OnGet(int id)
   {
      var projectResponse = await _significantChangeProjectRepository.GetProjectById(id);
      Id = id;
      SchoolName = projectResponse.Body?.SchoolName;
      SelectedDeliveryOfficer = projectResponse.Body?.AssignedUser?.FullName;

      DeliveryOfficers = await userRepository.GetAllUsers();

      return Page();
   }

   public async Task<IActionResult> OnPost(int id, string selectedName, bool unassignDeliveryOfficer, string deliveryOfficerInput)
   {
      ApiResponse<SignificantChangeProjectResponse> projectResponse = await _significantChangeProjectRepository.GetProjectById(id);
      if (string.IsNullOrWhiteSpace(deliveryOfficerInput))
      {
         selectedName = string.Empty;
      }

      if (unassignDeliveryOfficer)
      {
         await _significantChangeProjectRepository.SetAssignedUser(id, new SetAssignedUserSignificantChangeCommand(Guid.Empty, string.Empty, string.Empty));
         TempData.SetNotification(NotificationType.Success, "Done", "Project is unassigned");
      }
      else if (!string.IsNullOrEmpty(selectedName))
      {
         IEnumerable<User> deliveryOfficers = await userRepository.GetAllUsers();

         var assignedUser = deliveryOfficers.SingleOrDefault(u => u.FullName == selectedName);

         await _significantChangeProjectRepository.SetAssignedUser(id, new SetAssignedUserSignificantChangeCommand(new Guid(assignedUser.Id), assignedUser.FullName, assignedUser.EmailAddress));
         TempData.SetNotification(NotificationType.Success, "Done", "Project is assigned");
      }

      (string returnPage, string fragment) = GetReturnPageAndFragment();

      if (!string.IsNullOrWhiteSpace(returnPage))
      {
         return RedirectToPage(returnPage, null, new { id }, fragment);
      }
      else
      {
         return RedirectToPage(Links.SignificantChange.SignificantChangeTaskList.Page, new { id });
      }
   }

   private (string, string) GetReturnPageAndFragment()
   {
      Request.Query.TryGetValue("return", out StringValues returnQuery);
      Request.Query.TryGetValue("fragment", out StringValues fragmentQuery);
      return (returnQuery, fragmentQuery);
   }
}

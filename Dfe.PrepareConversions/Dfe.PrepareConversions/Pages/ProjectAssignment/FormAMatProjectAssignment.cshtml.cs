using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.ProjectAssignment;

public class FormAMatProjectAssignmentModel : PageModel
{
   private readonly IAcademyConversionProjectRepository _academyConversionProjectRepository;
   private readonly IUserRepository _userRepository;

   public FormAMatProjectAssignmentModel(IUserRepository userRepository, IAcademyConversionProjectRepository academyConversionProjectRepository)
   {
      _academyConversionProjectRepository = academyConversionProjectRepository;
      _userRepository = userRepository;
   }

   public string TrustName { get; private set; }
   public int Id { get; set; }
   public IEnumerable<User> DeliveryOfficers { get; set; }
   public string SelectedDeliveryOfficer { get; set; }

   public async Task<IActionResult> OnGet(int id)
   {
      ApiResponse<FormAMatProject> projectResponse = await _academyConversionProjectRepository.GetFormAMatProjectById(id);
      Id = id;
      TrustName = projectResponse.Body.ProposedTrustName;
      SelectedDeliveryOfficer = projectResponse.Body?.AssignedUser?.FullName;

      DeliveryOfficers = await _userRepository.GetAllUsers();

      return Page();
   }

   public async Task<IActionResult> OnPost(int id, string selectedName, bool unassignDeliveryOfficer, string deliveryOfficerInput)
   {
      ApiResponse<FormAMatProject> projectResponse = await _academyConversionProjectRepository.GetFormAMatProjectById(id);

      if (unassignDeliveryOfficer)
      {
         await _academyConversionProjectRepository.SetFormAMatAssignedUser(id, new SetAssignedUserModel(id, Guid.Empty, string.Empty, string.Empty));
         TempData.SetNotification(NotificationType.Success, "Done", "Project is unassigned");
      }
      else if (!string.IsNullOrEmpty(selectedName))
      {
         IEnumerable<User> deliveryOfficers = await _userRepository.GetAllUsers();

         var assignedUser = deliveryOfficers.SingleOrDefault(u => u.FullName == selectedName);

         await _academyConversionProjectRepository.SetFormAMatAssignedUser(id, new SetAssignedUserModel(id, new Guid(assignedUser.Id), assignedUser.FullName, assignedUser.EmailAddress));
         TempData.SetNotification(NotificationType.Success, "Done", "Project is assigned");
      }


      return RedirectToPage(Links.FormAMat.OtherSchoolsInMat.Page, new { id });
   }
}

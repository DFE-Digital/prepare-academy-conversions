using Dfe.PrepareConversions.Data;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareConversions.Data.Services;
using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Pages.ProjectAssignment
{
	public class IndexModel : PageModel
	{
		private readonly IUserRepository _userRepository;
		private readonly IAcademyConversionProjectRepository _academyConversionProjectRepository;
		public IndexModel(IUserRepository userRepository, IAcademyConversionProjectRepository academyConversionProjectRepository)
		{
			_academyConversionProjectRepository = academyConversionProjectRepository;
			_userRepository = userRepository;
		}

		public string SchoolName { get; private set; }
		public int Id { get; set; }
		public IEnumerable<User> DeliveryOfficers { get; set; }
		public string SelectedDeliveryOfficer { get; set; }

      public async Task<IActionResult> OnGet(int id)
		{
			var projectResponse = await _academyConversionProjectRepository.GetProjectById(id);
			Id = id;
			SchoolName = projectResponse.Body.SchoolName;
			SelectedDeliveryOfficer = projectResponse.Body?.AssignedUser?.FullName;

			DeliveryOfficers = await _userRepository.GetAllUsers();

			return Page();
		}

		public async Task<IActionResult> OnPost(int id, string selectedName, bool unassignDeliveryOfficer, string deliveryOfficerInput)
      {

         if (string.IsNullOrWhiteSpace(deliveryOfficerInput))
         {
            selectedName = string.Empty;
         }

         if (unassignDeliveryOfficer)
			{
				var updatedProject = new UpdateAcademyConversionProject
				{
					AssignedUser = new User(Guid.Empty.ToString(), string.Empty, string.Empty)
				};

				await _academyConversionProjectRepository.UpdateProject(id, updatedProject);
				TempData.SetNotification(NotificationType.Success, "Done", "Project is unassigned");
			}
			else if (!string.IsNullOrEmpty(selectedName))
			{
				var deliveryOfficers = await _userRepository.GetAllUsers();
				var updatedProject = new UpdateAcademyConversionProject
				{
               AssignedUser = deliveryOfficers.SingleOrDefault(u => u.FullName == selectedName)
				};

				await _academyConversionProjectRepository.UpdateProject(id, updatedProject);
				TempData.SetNotification(NotificationType.Success, "Done", "Project is assigned");
			}

			return RedirectToPage(Links.TaskList.Index.Page, new { id });
		}
	}
}
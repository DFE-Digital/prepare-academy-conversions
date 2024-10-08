using Dfe.PrepareConversions.Data.Services.Interfaces;
using Dfe.PrepareConversions.Data.Models;
using Dfe.PrepareTransfers.Data;
using Dfe.PrepareTransfers.Web.ExtensionMethods;
using Dfe.PrepareTransfers.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dfe.PrepareTransfers.Web.Pages.Projects.ProjectAssignment
{
 public class IndexModel : PageModel
    {
		private readonly IUserRepository _userRepository;
		private readonly IProjects _projectRepository;
		public IndexModel(IUserRepository userRepository, IProjects projectRepository)
		{
			_projectRepository = projectRepository;
			_userRepository = userRepository;
		}

		public string IncomingTrustName { get; private set; }
		public string Urn { get; set; }
		public IEnumerable<PrepareConversions.Data.Models.User> DeliveryOfficers { get; set; }
		public string SelectedDeliveryOfficer { get; set; }

		public async Task<IActionResult> OnGetAsync(string urn)
		{
			var projectResponse = await _projectRepository.GetByUrn(urn);
			Urn = urn;
			IncomingTrustName = projectResponse.Result.IncomingTrustName;
			SelectedDeliveryOfficer = projectResponse.Result?.AssignedUser?.FullName;
			DeliveryOfficers = await _userRepository.GetAllUsers();

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(string urn, string selectedName, bool unassignDeliveryOfficer = false)
		{
			var project = (await _projectRepository.GetByUrn(urn)).Result;

			if (unassignDeliveryOfficer)
			{
				project.AssignedUser = new PrepareConversions.Data.Models.User(Guid.Empty.ToString(), string.Empty, string.Empty);

				await _projectRepository.AssignUser(project);

				TempData.SetNotification("Done", "Project is unassigned");
			}
			else if (!string.IsNullOrEmpty(selectedName))
			{
				var deliveryOfficers = await _userRepository.GetAllUsers();

				project.AssignedUser = deliveryOfficers.SingleOrDefault(u => u.FullName == selectedName);

				await _projectRepository.AssignUser(project);

				TempData.SetNotification("Done", "Project is assigned");
			}

			return RedirectToPage(Links.Project.Index.PageName, new { urn });
		}
	}
}

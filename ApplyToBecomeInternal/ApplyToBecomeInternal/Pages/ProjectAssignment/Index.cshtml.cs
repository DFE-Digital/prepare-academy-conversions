using ApplyToBecome.Data;
using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services;
using ApplyToBecome.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages.ProjectAssignment
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
		public IEnumerable<User> DeliveryOfficers { get; set; }
        public async Task OnGet(int id)
        {
			
			ApiResponse<AcademyConversionProject> projectResponse = await _academyConversionProjectRepository.GetProjectById(id);
			SchoolName = projectResponse.Body.SchoolName;


			// TODO: Temp users
			var users = await _userRepository.SearchUsers("A");
	        DeliveryOfficers = users;
        }
    }
}

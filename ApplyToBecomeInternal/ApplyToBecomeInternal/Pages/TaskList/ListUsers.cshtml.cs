using ApplyToBecome.Data.Models;
using ApplyToBecome.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplyToBecomeInternal.Pages
{
    public class ListUsersModel : PageModel
    {
		private readonly IUserRepository _userRepository;

		public List<User> Users { get; set; }

		public ListUsersModel(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

        public async Task<IActionResult> OnGet(string searchString)
        {
			Users = (await _userRepository.SearchUsers(searchString)).ToList();

			return Page();
		}
    }
}

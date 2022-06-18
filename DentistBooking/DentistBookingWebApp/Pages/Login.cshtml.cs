using BusinessObject;
using DataAccess.Interfaces;
using DentistBookingWebApp.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace DentistBookingWebApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserRepository userRepository;
        public LoginModel(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        [BindProperty]
        [Required]
        [EmailAddress(ErrorMessage = "Wrong format for email address")]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                int userId = userRepository.Login(Email, HashCode.HashPassword(Password));
                if (userId > 0)
                {
                    User user = userRepository.GetUserById(userId);
                    HttpContext.Session.SetString("EMAIL", user.Email);
                    HttpContext.Session.SetString("ROLE", user.Role.Name.ToString());
                    return RedirectToPage("./Index");
                }
                else
                {
                    ViewData["ErrorMessage"] = "Wrong email or password.";
                    return Page();
                }
            }
            else
            {
                return Page();
            }
        }
    }
}

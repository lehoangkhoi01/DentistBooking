using BusinessObject;
using DataAccess.Interfaces;
using DentistBookingWebApp.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

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
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public IActionResult OnGet(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/Index");
            if (ModelState.IsValid)
            {
                int userId = userRepository.Login(Email, HashCode.HashPassword(Password));
                if (userId > 0)
                {
                    User user = userRepository.GetUserById(userId);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, user.Role.Name),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())                       
                    };
                    var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                    };
                    //var identityClaim = new Claim(ClaimTypes.Email, user.Email);
                    //var roleClaim = new Claim(ClaimTypes.Role, user.Role.Name);
                    //HttpContext.Session.SetString("EMAIL", user.Email);
                    //HttpContext.Session.SetString("ROLE", user.RoleId.ToString());
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                    new ClaimsPrincipal(claimsIdentity),
                                                    authProperties);

                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
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

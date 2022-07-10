using BusinessObject;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DentistBookingWebApp.Pages.Account.Edit
{
    [Authorize(Roles = "Admin")]
    public class AdminModel : PageModel
    {
        private readonly IAdminRepository adminRepository;
        private readonly IUserRepository userRepository;

        public AdminModel(IAdminRepository adminRepository, IUserRepository userRepository)
        {
            this.adminRepository = adminRepository;
            this.userRepository = userRepository;
        }
        [BindProperty]
        public ViewModels.Admin Admin { get; set; }

        [BindProperty]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Maximum length for passwod is 50 characters")]
        public string Password { get; set; }

        [BindProperty]
        public string Email { get; set; }


        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (userId != id)
            {
                return NotFound();
            }

            try
            {
                BusinessObject.Admin admin = adminRepository.GetAdminByUserId((int)id);
                if (admin == null)
                {
                    return NotFound();
                }

                Admin = new ViewModels.Admin
                {
                    Id = admin.Id,
                    FullName = admin.FullName,
                    PhoneNumber = admin.PhoneNumber,
                    UserId = admin.UserId,
                };
                Email = admin.User.Email;

            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "There is an error. Please try again later.";
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                BusinessObject.Admin admin = new BusinessObject.Admin
                {
                    Id = Admin.Id,
                    FullName = Admin.FullName,
                    PhoneNumber = Admin.PhoneNumber,
                    UserId = Admin.UserId,
                };
                if (!string.IsNullOrEmpty(Password))
                {
                    User user = userRepository.GetUserById(Admin.UserId);
                    user.Password = Validation.HashCode.HashPassword(Password);
                    userRepository.Update(user);
                }
                adminRepository.UpdateAdmin(admin);
                TempData["Message"] = "Update information successfully";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "There is an error. Please try again later.";
            }

            return Page();
        }
    }
}

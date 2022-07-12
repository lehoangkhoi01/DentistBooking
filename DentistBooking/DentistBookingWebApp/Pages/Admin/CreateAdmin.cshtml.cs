using DataAccess.Interfaces;
using DentistBookingWebApp.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using HashCode = DentistBookingWebApp.Validation.HashCode;

namespace DentistBookingWebApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class CreateAdminModel : PageModel
    {
        private readonly IAdminRepository adminRepository;
        private readonly IUserRepository userRepository;

        public CreateAdminModel(IAdminRepository adminRepository, 
                                IUserRepository userRepository)
        {
            this.adminRepository = adminRepository;
            this.userRepository = userRepository;
        }


        [BindProperty]
        public ViewModels.Admin AdminViewModel { get; set; }
        [BindProperty]
        public ViewModels.User UserViewModel { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPostAsync()
        {
            try
            {
                if (SignUpValidation.CheckEmail(UserViewModel.Email))
                {
                    ModelState.AddModelError("UserViewModel.Email", "This email already exists");
                }
                if (SignUpValidation.CheckPhoneAdmin(AdminViewModel.PhoneNumber))
                {
                    ModelState.AddModelError("AdminViewModel.PhoneNumber", "This phone number already exists");
                }
                if (ModelState.IsValid)
                {
                    BusinessObject.User userObj = new BusinessObject.User
                    {
                        Email = UserViewModel.Email,
                        Password = HashCode.HashPassword(UserViewModel.Password),
                        RoleId = 1
                    };
                    int userId = userRepository.SignUp(userObj);
                    if (userId > 0)
                    {
                        BusinessObject.Admin admin = new BusinessObject.Admin
                        {
                            UserId = userId,
                            FullName = AdminViewModel.FullName,
                            PhoneNumber = AdminViewModel.PhoneNumber
                        };
                        adminRepository.AddAdmin(admin);
                        TempData["Message"] = "Add new admin successfully";
                        return RedirectToPage("/Admin/Account");
                    }
                }
                return Page();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "There is an error. Please try again later";
                return Page();
            }

        }
    }
}

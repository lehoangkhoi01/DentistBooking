using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;

namespace DentistBookingWebApp.Pages.Account.Details
{
    [Authorize(Roles = "Admin")]
    public class AdminModel : PageModel
    {
        private readonly IAdminRepository adminRepository;
        public AdminModel(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }

        public ViewModels.Admin Admin { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
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
                    User = new ViewModels.User
                    {
                        Id = admin.User.Id,
                        Password = admin.User.Password,
                        RoleId = admin.User.RoleId,
                        Email = admin.User.Email,
                        Role = admin.User.Role
                    }
                };

                return Page();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "There is an error. Please try again";
                return RedirectToPage("/Index");
            }
        }
    }
}

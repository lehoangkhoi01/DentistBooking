using BusinessObject;
using DataAccess.Interfaces;
using DentistBookingWebApp.Utils.FileUploadService;
using DentistBookingWebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace DentistBookingWebApp.Pages.Services
{
    public class EditModel : PageModel
    {
        private readonly IServiceRepository serviceRepository;
        private readonly IUserRepository userRepository;
        private readonly IAdminRepository adminRepository;
        private readonly IFileUploadService fileUploadService;

        public EditModel(IServiceRepository _serviceRepository,
                            IUserRepository _userRepository,
                            IAdminRepository _adminRepository,
                            IFileUploadService _fileUploadService)
        {
            serviceRepository = _serviceRepository;
            userRepository = _userRepository;
            adminRepository = _adminRepository;
            fileUploadService = _fileUploadService;
        }

        [BindProperty]
        public ServiceViewModel ServiceViewModel { get; set; }

        public IActionResult OnGet(int? id)
        {
            //Authorize and valiadation
            string roleId = HttpContext.Session.GetString("ROLE");
            if(string.IsNullOrEmpty(roleId))
            {
                return RedirectToPage("/Login");
            }
            else if(roleId != "1" || id == null)
            {
                return NotFound();
            }
            try
            {
                Service service = serviceRepository.GetServiceById((int)id);
                if (service == null)
                {
                    return NotFound();
                }

                ServiceViewModel = new ServiceViewModel
                {
                    Id = service.Id,
                    Description = service.Description,
                    Name = service.Name,
                    Price = service.Price,
                    Image = service.Image,
                    CreatedDate = service.CreatedDate,
                    Status = service.Status == "Active" ? true : false
                };
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("./Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                BusinessObject.User user = userRepository.GetUserByEmail(HttpContext.Session.GetString("EMAIL"));
                BusinessObject.Admin admin = adminRepository.GetAdminByUserId(user.Id);

                Service service = new Service
                {
                    Id = ServiceViewModel.Id,
                    Name = ServiceViewModel.Name,
                    Description = ServiceViewModel.Description,
                    Price = ServiceViewModel.Price,
                    Image = ServiceViewModel.Image,
                    CreatedDate = ServiceViewModel.CreatedDate,
                    UpdatedDate = DateTime.Now,
                    Status = ServiceViewModel.Status ? "Active" : "Inactive",
                    CreatedPersonId = admin.Id,
                };

                if(ServiceViewModel.ImageFile != null)
                {
                    await fileUploadService.UploadFileAsync(ServiceViewModel.ImageFile);
                    service.Image = ServiceViewModel.ImageFile.FileName;
                }
                serviceRepository.UpdateService(service);
                TempData["Message"] = "Update service successfully";
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
            return RedirectToPage("Edit", "Get", new {id = ServiceViewModel.Id});
            
        }
    }
}

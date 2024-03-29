using BusinessObject;
using DataAccess.Interfaces;
using DentistBookingWebApp.Utils.FileUploadService;
using DentistBookingWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Claims;
using System.Threading.Tasks;


namespace DentistBookingWebApp.Pages.Services
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly IServiceRepository serviceRepository;
        private readonly IUserRepository userRepository;
        private readonly IAdminRepository adminRepository;
        private readonly IFileUploadService fileUploadService;
        public CreateModel(IServiceRepository _serviceRepository,
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

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var _service = serviceRepository.GetServiceByName(ServiceViewModel.Name);
                if (_service == null) //Check if service is already existed
                {
                    int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    BusinessObject.Admin admin = adminRepository.GetAdminByUserId(userId);

                    string filePath = await fileUploadService.UploadFileAsync(ServiceViewModel.ImageFile);
                    Service service = new Service
                    {
                        Name = ServiceViewModel.Name,
                        Description = ServiceViewModel.Description,
                        Price = ServiceViewModel.Price,
                        Image = ServiceViewModel.ImageFile.FileName,
                        CreatedDate = DateTime.Now.Date,
                        UpdatedDate = DateTime.Now.Date,
                        CreatedPersonId = admin.Id,
                        Status = "Active"
                    };
                    serviceRepository.AddNewService(service);
                    TempData["Message"] = "Add new service successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "This serice is already existed.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }

            
            return RedirectToPage("./Index");
        }
    }
}

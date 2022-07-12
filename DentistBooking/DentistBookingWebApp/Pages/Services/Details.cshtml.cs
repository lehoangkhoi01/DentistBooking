using BusinessObject;
using DataAccess.Interfaces;
using DentistBookingWebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DentistBookingWebApp.Pages.Services
{
    public class DetailsModel : PageModel
    {
        private readonly IServiceRepository serviceRepository;

        public DetailsModel(IServiceRepository serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }

        [BindProperty]
        public ServiceViewModel serviceViewModel { get; set; }

        public IList<Service> TopServiceList { get; set; }

        [BindProperty]
        public string Role { get; set; }
        public string Status { get; set; }


        public IActionResult OnGet(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Role = User.FindFirst(claim => claim.Type == ClaimTypes.Role)?.Value;


            Service service = serviceRepository.GetServiceById((int)id);
            TopServiceList = serviceRepository.GetActiveServiceList().Take(4).ToList();

            if(service == null)
            {
                return NotFound();
            }
            else
            {
                serviceViewModel = new ServiceViewModel
                {
                    Id = service.Id,
                    Name = service.Name,
                    Description = service.Description,
                    Image = service.Image,
                    Price = service.Price,
                    Status = service.Status == "Active" ? true : false,
                    Admin = service.Admin,
                    CreatedDate = service.CreatedDate,
                    UpdatedDate = service.UpdatedDate
                };

                return Page();
            }
        }

        //public IActionResult OnPostDisableService([FromForm] int serviceId)
        //{
        //    try
        //    {
        //        Service _service = serviceRepository.GetServiceById(serviceId);
        //        if (_service == null)
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            Service service = new Service
        //            {
        //                Id = _service.Id,
        //                Description = _service.Description,
        //                Name = _service.Name,
        //                Price = _service.Price,
        //                Image = _service.Image,
        //                CreatedDate = _service.CreatedDate,
        //                UpdatedDate = DateTime.Now,
        //                Admin = _service.Admin,
        //                CreatedPersonId = _service.CreatedPersonId,
        //                Status = "Inactive"
        //            };
        //            serviceRepository.UpdateService(service);
        //            TempData["Message"] = "Disable service successfully";
        //            return RedirectToPage("/Services/Index");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        TempData["ErrorMessage"] = "There is an error. Please try again later";
        //        return Page();
        //    }
        //}
    }
}

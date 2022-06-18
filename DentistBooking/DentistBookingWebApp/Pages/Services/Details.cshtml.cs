using BusinessObject;
using DataAccess.Interfaces;
using DentistBookingWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DentistBookingWebApp.Pages.Services
{
    public class DetailsModel : PageModel
    {
        private readonly IServiceRepository serviceRepository;

        public DetailsModel(IServiceRepository serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }


        public ServiceViewModel serviceViewModel { get; set; }
        public string Role { get; set; }


        public IActionResult OnGet(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Service service = serviceRepository.GetServiceById(id);

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
                    Status = service.Status,
                    Admin = service.Admin
                };
            }
            return Page();
        }
    }
}

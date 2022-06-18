using BusinessObject;
using DataAccess.Interfaces;
using DentistBookingWebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace DentistBookingWebApp.Pages.Services
{
    public class IndexModel : PageModel
    {
        private readonly IServiceRepository serviceRepository;
        public IndexModel(IServiceRepository _serviceRepository)
        {
            serviceRepository = _serviceRepository;
        }

        [BindProperty]
        public IList<ServiceViewModel> serviceList { get; set; }
        public string Role { get; set; }

        public void OnGet()
        {
            string role = HttpContext.Session.GetString("ROLE");
            if(!string.IsNullOrEmpty(role))
            {
                Role = role;
            }

            List<Service> services = serviceRepository.GetServiceList().ToList();
            foreach(Service service in services)
            {
                ServiceViewModel serviceViewModel = new ServiceViewModel
                {
                    Id = service.Id,
                    Name = service.Name,
                    Price = service.Price,
                    Status = service.Status,
                    Image = service.Image,
                    CreatedDate = service.CreatedDate,
                    UpdatedDate = service.UpdatedDate,
                    Description = service.Description,
                    Admin = service.Admin
                };
                serviceList.Add(serviceViewModel);
            }
        }
    }
}

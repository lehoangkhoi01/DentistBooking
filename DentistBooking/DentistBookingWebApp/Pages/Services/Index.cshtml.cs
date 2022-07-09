using BusinessObject;
using DataAccess.Interfaces;
using DentistBookingWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DentistBookingWebApp.Pages.Services
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private const int MAX_ITEM_PAGE = 4;
        private readonly IServiceRepository serviceRepository;


        public IndexModel(IServiceRepository _serviceRepository)
        {
            serviceRepository = _serviceRepository;
        }

        [BindProperty]
        public IList<ServiceViewModel> serviceList { get; set; }
        public string Role { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public IActionResult OnGet([FromQuery] int? page = 1)
        {
            Role = User.FindFirst(claim => claim.Type == ClaimTypes.Role)?.Value;

            try
            {
                List<Service> services = serviceRepository
                            .GetActiveServiceListByPage((int)page, MAX_ITEM_PAGE)
                            .ToList();

                if (serviceList == null)
                {
                    serviceList = new List<ServiceViewModel>();
                }

                int pageCount;
                if (!string.IsNullOrEmpty(SearchString))
                {
                    services = services.Where(s => s.Name.Contains(SearchString)).ToList();
                    pageCount = (int)Math.Ceiling(services.Count() / (double)MAX_ITEM_PAGE);

                }
                else
                {
                    pageCount = (int)Math.Ceiling(serviceRepository.GetActiveServiceList().Count() / (double)MAX_ITEM_PAGE);
                }

                if (page <= 0 || page > pageCount)
                {
                    return NotFound();
                }
                ViewData["PageCount"] = pageCount;
                ViewData["CurrentPage"] = page;

                foreach (Service service in services)
                {
                    ServiceViewModel serviceViewModel = new ServiceViewModel
                    {
                        Id = service.Id,
                        Name = service.Name,
                        Price = service.Price,
                        Status = service.Status == "Active" ? true : false,
                        Image = service.Image,
                        CreatedDate = service.CreatedDate,
                        UpdatedDate = service.UpdatedDate,
                        Description = service.Description,
                        Admin = service.Admin
                    };
                    serviceList.Add(serviceViewModel);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("/Index");
            }

            return Page();
            
        }

        
    }
}

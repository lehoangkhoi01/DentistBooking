using BusinessObject;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace DentistBookingWebApp.ViewModels
{
    public class ServiceViewModel
    {
        [Display(Name = "Service's ID" )]
        public int Id { get; set; }

        [Required(ErrorMessage = "The service name can not be empty.")]
        [Display(Name = "Service's name")]
        [StringLength(maximumLength: 50, ErrorMessage = "The service name's length should be between 6-50 characters.", MinimumLength = 6)]
        public string Name { get; set; }


        public string Image { get; set; }
        [Required(ErrorMessage = "Please choose an image for this service")]
        [DataType(DataType.Upload)]
        [Display(Name = "Choose an image for this service")]

        public IFormFile ImageFile { get; set; }


        [Display(Name = "Description")]
        [Required(ErrorMessage = "The description can not be empty")]
        [StringLength(maximumLength: 200, ErrorMessage = "The description's length should be between 20-200 characters.", MinimumLength = 20)]
        public string Description { get; set; }


        [Display(Name = "Service's price")]
        public double Price { get; set; }


        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime UpdatedDate { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        public Admin Admin { get; set; }
    }
}

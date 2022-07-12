using BusinessObject;
using DentistBookingWebApp.Validation;
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


        [DataType(DataType.Upload)]
        [Display(Name = "Choose an image for this service")]
        [ImageValidation(ErrorMessage = "Wrong format for image")]
        public IFormFile ImageFile { get; set; }


        [Display(Name = "Description")]
        [Required(ErrorMessage = "The description can not be empty")]
        [StringLength(maximumLength: 2000, ErrorMessage = "The description's length should be between 20-2000 characters.", MinimumLength = 20)]
        public string Description { get; set; }


        [Display(Name = "Service's price")]
        [Range(0, 1000, ErrorMessage = "Range for service price should be between 0 - 1000")]
        public double Price { get; set; }


        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime UpdatedDate { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        public BusinessObject.Admin Admin { get; set; }
    }
}

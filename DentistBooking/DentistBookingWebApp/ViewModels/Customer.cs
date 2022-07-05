using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBookingWebApp.ViewModels
{
    public class Customer
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Name must have from 6 to 50 characters", MinimumLength = 6)]
        [Display(Name = "Full name")]
        public string FullName { get; set; }
        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessage = "Phone number must contain 10 characters", MinimumLength = 10)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [ForeignKey("Id")]
        public int UserId { get; set; }
        public BusinessObject.User User { get; set; }

    }
}

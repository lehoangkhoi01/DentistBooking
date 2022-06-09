using BusinessObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistBookingWebApp.ViewModels
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password length must be between 6 and 50", MinimumLength = 6)]


        public string Password { get; set; }

        [ForeignKey("Id")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DentistBookingWebApp.ViewModels
{
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full name")]
        [StringLength(50, ErrorMessage = "Name must have from 6 to 50 characters", MinimumLength = 6)]
        public string FullName { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessage = "Phone number must contain 10 characters", MinimumLength = 10)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}

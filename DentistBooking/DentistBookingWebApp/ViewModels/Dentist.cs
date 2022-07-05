using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentistBookingWebApp.ViewModels
{
    public class Dentist
    {
        [Key]
        public int Id { get; set; }
        [Key]
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

        [ForeignKey("Id")]
        public int UserId { get; set; }
        public BusinessObject.User User { get; set; }
    }
}

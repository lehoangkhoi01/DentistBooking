using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime ResevrationDate { get; set; }
        public double Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Status { get; set; }

        [ForeignKey("Id")]
        public int DentistId { get; set; }
        public virtual Dentist Dentist { get; set; }

        [ForeignKey("Id")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey("Id")]
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; }



            
    }
}

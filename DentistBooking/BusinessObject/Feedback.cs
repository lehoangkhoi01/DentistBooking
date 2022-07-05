using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Feedback
    {
        public int Id { get; set; }
        public int Star { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [ForeignKey("Id")]
        public int ReservationId { get; set; }
        public virtual Reservation Reservation { get; set; }

        [ForeignKey("Id")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}

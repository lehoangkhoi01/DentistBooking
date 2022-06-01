using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [ForeignKey("Id")]
        public int StatusId { get; set; }
        public Status Status { get; set; }

        [ForeignKey("Id")]
        public int CreatedPersonId { get; set; }
        public Admin Admin { get; set; }

    }
}

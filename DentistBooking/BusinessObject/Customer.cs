using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Customer
    {

        public int Id { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        [ForeignKey("Id")]
        public int UserId { get; set; }
        public User User { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class User
    {

        public int Id { get; set; }
        
        public string Email { get; set; }

        public string Password { get; set; }

        [ForeignKey("Id")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}

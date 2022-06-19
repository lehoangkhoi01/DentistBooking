using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IAdminRepository
    {
        public Admin GetAdminByUserId(int userId);
    }
}

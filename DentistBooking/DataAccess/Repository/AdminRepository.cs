using BusinessObject;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AdminRepository : IAdminRepository
    {
        public Admin GetAdminByUserId(int userId) => AdminDAO.Instance.GetAdminByUserId(userId);

        public void UpdateAdmin(Admin admin) => AdminDAO.Instance.Update(admin);
    }
}

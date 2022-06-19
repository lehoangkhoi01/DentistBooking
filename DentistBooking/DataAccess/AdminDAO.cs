using BusinessObject;
using BusinessObject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AdminDAO
    {
        private static AdminDAO instance = null;
        private static readonly object instanceLock = new object();

        private AdminDAO() { }
        public static AdminDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AdminDAO();
                    }
                }
                return instance;
            }
        }
        //------------------------------------------

        public Admin GetAdminByUserId(int userId)
        {
            Admin admin;
            try
            {
                var dbContext = new DentistBookingContext();
                admin = dbContext.Admins.FirstOrDefault(a => a.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return admin;
        }
    }
}

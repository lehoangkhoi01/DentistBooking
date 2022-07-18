using BusinessObject;
using BusinessObject.Data;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<Admin> GetAdminList()
        {
            IEnumerable<Admin> admins;
            try
            {
                var dbContext = new DentistBookingContext();
                admins = dbContext.Admins.Include(c => c.User.Role).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return admins;
        }
        public Admin GetAdminByUserId(int userId)
        {
            Admin admin;
            try
            {
                var dbContext = new DentistBookingContext();
                admin = dbContext.Admins.Include(a => a.User).FirstOrDefault(a => a.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return admin;
        }

        public Admin GetAdminByPhone(string phone)
        {
            Admin admin;
            try
            {
                var dbContext = new DentistBookingContext();
                admin = dbContext.Admins.Include(d => d.User).FirstOrDefault(x => x.PhoneNumber == phone);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return admin;
        }
        public void Update(Admin admin)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                dbContext.Entry<Admin>(admin).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Add(Admin admin)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                dbContext.Admins.Add(admin);
                dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

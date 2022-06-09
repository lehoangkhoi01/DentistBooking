using BusinessObject;
using BusinessObject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserDAO
    {
        private static UserDAO instance = null;
        private static readonly object instanceLock = new object();

        private UserDAO() { }
        public static UserDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new UserDAO();
                    }
                }
                return instance;
            }
        }
        //--------------------------------------------------

        public int Login(string username, string password)
        {
            int result = 0;
            try
            {
                var dbContext = new DentistBookingContext();
                User user = dbContext.Users.FirstOrDefault(u => u.Email.Equals(username) && u.Password.Equals(password));
                if (user != null)
                {
                    result = user.Id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public int Signup(User user)
        {
            int result = 0;
            try
            {
                var dbContext = new DentistBookingContext();
                User _user = dbContext.Users.FirstOrDefault(u => u.Email.Equals(user.Email));
                if (_user == null)
                {
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                    result = user.Id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public User GetUser(int id)
        {
            User user;
            try
            {
                var dbContext = new DentistBookingContext();
                user = dbContext.Users.FirstOrDefault(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }
        public User GetUserByEmail(string email)
        {
            User user;
            try
            {
                var dbContext = new DentistBookingContext();
                user = dbContext.Users.FirstOrDefault(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }

    }
}

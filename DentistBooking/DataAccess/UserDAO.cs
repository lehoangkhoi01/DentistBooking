using BusinessObject;
using BusinessObject.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


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

        public void UpdateUser(User user)
        {
            try
            {
                var dbContext = new DentistBookingContext();
                dbContext.Entry<User>(user).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public User GetUser(int id)
        {
            User user;
            try
            {
                var dbContext = new DentistBookingContext();
                user = dbContext.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == id);
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
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = new List<User>();
            try
            {
                var dbContext = new DentistBookingContext();
                users = dbContext.Users.Include(u => u.Role).ToList();
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return users;
        }

    }
}

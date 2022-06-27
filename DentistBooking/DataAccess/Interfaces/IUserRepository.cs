using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Return User ID if login successfully
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int Login(string username, string password);

        /// <summary>
        /// Return User ID if signup successfully
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int SignUp(User user);

        /// <summary>
        /// Return User by user's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetUserById(int id);
        public User GetUserByEmail(string email);
        public IEnumerable<User> GetUsers();

    }
}

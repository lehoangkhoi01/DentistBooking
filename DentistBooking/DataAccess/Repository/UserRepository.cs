using BusinessObject;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        public int Login(string username, string password) => UserDAO.Instance.Login(username, password);

        public int SignUp(User user) => UserDAO.Instance.Signup(user);

        public User GetUserById(int id) => UserDAO.Instance.GetUser(id);

        public User GetUserByEmail(string email) => UserDAO.Instance.GetUserByEmail(email);

        public IEnumerable<User> GetUsers()
        => UserDAO.Instance.GetUsers();

        public void Update(User user) => UserDAO.Instance.UpdateUser(user);
    }
}

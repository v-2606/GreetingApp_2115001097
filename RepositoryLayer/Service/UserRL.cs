using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class UserRL :IUserRL
    {

        private readonly UserContext _context;

        public UserRL(UserContext context)
        {
            _context = context;
        }

        public bool Register(UsersEntity user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }

        public UsersEntity GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

    }
}

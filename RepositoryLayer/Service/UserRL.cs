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
    public class UserRL : IUserRL
    {

        private readonly UserContext _context;

        public UserRL(UserContext context)
        {
            _context = context;
        }

        public bool Register(UsersEntity user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DB Error: {ex.InnerException?.Message ?? ex.Message}");
                throw new Exception("Database Save Error: " + ex.InnerException?.Message ?? ex.Message);
            }
        }


        public UsersEntity GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
        public bool SaveToken(int userId, string token)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.JwtToken = token;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void SaveResetToken(int userId, string resetToken)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.ResetToken = resetToken;
                user.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(5);
                _context.SaveChanges();
            }
        }


        public UsersEntity? GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public bool UpdatePassword(int userId, byte[] newPasswordHash)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return false;

            user.PasswordHash = newPasswordHash;
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            _context.SaveChanges();
            return true;
        }

        public bool ResetPassword(int userId, byte[] newPasswordHash)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return false;

            user.PasswordHash = newPasswordHash;
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            _context.SaveChanges();
            return true;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessLayer.Interface;
using ModelLayer.DTO;
using RepositoryLayer.Entity;

using RepositoryLayer.Interface;
using RepositoryLayer.Service;

using RepositoryLayer.Helper;

namespace BussinessLayer.Service
{
    public class UserBL : IUserBL
    {

        private readonly IUserRL _userRL;
        private readonly PasswordHashing _PasswordHashing;
        public UserBL(IUserRL userRL, PasswordHashing PasswordHashing)
        {
            _userRL = userRL;
            _PasswordHashing = PasswordHashing;
        }
        public bool Register(RegisterDTO model)
        {
            try
            {
               
                var existingUser = _userRL.GetUserByEmail(model.Email);
                if (existingUser != null)
                {
                    throw new Exception("User already registered.");
                }

               
                byte[] hashedPassword = PasswordHashing.HashPassword(model.Password);

              
                UsersEntity user = new()
                {
                    Name = model.Name,
                    Email = model.Email,
                    PasswordHash = hashedPassword,
                };

                
                return _userRL.Register(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Register method: " + ex.Message);
            }
        }


        public bool Login(LoginDTO model)
        {
            try
            {
                var user = _userRL.GetUserByEmail(model.Email);
                if (user == null) return false;

                return PasswordHashing.VerifyPassword(model.Password, user.PasswordHash);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Login method: " + ex.Message);
            }
        }

    }
}
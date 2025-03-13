using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.DTO;
using RepositoryLayer.Entity;

namespace BussinessLayer.Interface
{
    public interface IUserBL
    {
        bool Register(RegisterDTO model);
        
        string Login(LoginDTO model);

        bool ForgotPassword(string email);
        bool ResetPassword(string token, string newPassword);
        UsersEntity GetUserByEmail(string email); 
        void SaveResetToken(int userId, string resetToken);
        bool UpdatePassword(int userId, byte[] newPasswordHash);

        UsersEntity? ValidateResetToken(string token);
    }
}

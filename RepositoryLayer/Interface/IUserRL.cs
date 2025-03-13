using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public  interface  IUserRL
    {

        UsersEntity GetUserByEmail(string email);
        bool Register(UsersEntity user);


        bool SaveToken(int userId, string token);
        void SaveResetToken(int userId, string token);

        bool ResetPassword(int userId, byte[] newPasswordHash);

        bool UpdatePassword(int userId, byte[] newPasswordHash);
        public UsersEntity? GetUserById(int userId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.DTO;

namespace BussinessLayer.Interface
{
    public interface IUserBL
    {
        bool Register(RegisterDTO model);
        
        bool Login(LoginDTO model);
    }
}

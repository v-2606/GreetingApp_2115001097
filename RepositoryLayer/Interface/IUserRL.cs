﻿using System;
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
    }
}

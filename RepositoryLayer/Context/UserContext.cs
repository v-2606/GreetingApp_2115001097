﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public  class UserContext:DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        { 
        }
    public virtual DbSet<GreetingEntity> Greetings { get; set; }
        public virtual DbSet<UsersEntity> Users { get; set; }


    }
}

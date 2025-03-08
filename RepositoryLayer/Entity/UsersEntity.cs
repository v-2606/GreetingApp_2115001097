using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class UsersEntity
    {
        
            [Key]  // Primary Key
            public int Id { get; set; }
        
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public byte[] PasswordHash { get; set; }


        // Relationship: One User -> Many Greetings
        public ICollection<GreetingEntity> Greetings { get; set; } = new List<GreetingEntity>();
        }


    }


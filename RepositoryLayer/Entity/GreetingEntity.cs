using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RepositoryLayer.Entity
{
    public class GreetingEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Default value

        [ForeignKey("Users")]
        public int UserId { get; set; }

        
        public UsersEntity Users { get; set; }
    }
}

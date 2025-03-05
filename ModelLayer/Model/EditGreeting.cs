using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
   public  class EditGreeting
    {
        [Required]
        public string NewMessage { get; set; }
    }
}

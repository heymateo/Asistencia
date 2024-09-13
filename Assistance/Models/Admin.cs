using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistance.Models
{
    public class Admin
    {
        [Key]
        public int Id_Admin { get; set; }   
        public string User { get; set; }
        public string Password { get; set; }    
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistance.Models
{
    public class PasswordResets
    {
        [Key]
        public int Id_Password { get; set; }
        public string Email { get; set; }
        public string Token {  get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}

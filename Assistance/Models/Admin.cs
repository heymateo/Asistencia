using System;
using System.ComponentModel.DataAnnotations;

namespace Assistance.Models
{
#nullable enable
    public class Admin
    {
        [Key]
        public int Id_Admin { get; set; }
        [Required(ErrorMessage = "Escriba el nombre de usuario")]
        public string User { get; set; } = string.Empty;
        [Required(ErrorMessage = "Escriba su correo")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Escriba su contraseña")]
        public string Password { get; set; } = string.Empty;
        public string? Salt { get; set; }
        public string? PasswordResetToken { get; set; }  
        public DateTime? PasswordResetTokenExpiry { get; set; }
    }
}

using System.Text.RegularExpressions;

namespace Assistance.Services
{
    public class EmailValidationService
    {
        //private readonly string emailPattern = @"^[^@\s]+@mep\.go\.cr$";
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            // Validar el formato del correo 
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}

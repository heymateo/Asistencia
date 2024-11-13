using System;
using System.Security.Cryptography;

namespace Assistance.Services
{
    public class PasswordResetTokenService
    {
        public string GeneratePasswordResetToken()
        {
            var tokenData = new byte[32];  
            RandomNumberGenerator.Fill(tokenData);
            return Convert.ToBase64String(tokenData);
        }
    }
}

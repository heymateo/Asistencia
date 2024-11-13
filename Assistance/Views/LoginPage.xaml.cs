using System;
using Microsoft.UI.Xaml;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using Assistance.Services;
using Microsoft.Extensions.DependencyInjection;
using Assistance.Models;
using System.Security.Cryptography;
using System.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Assistance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private readonly AssistanceDbContext _context;
        private readonly AdminSessionService _adminSessionService;

        public LoginPage()
        {
            this.InitializeComponent();

            _context = ((App)Application.Current).Services.GetRequiredService<AssistanceDbContext>();

            _adminSessionService = ((App)Application.Current).Services.GetRequiredService<AdminSessionService>();

        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            var restorePasswordWindow = new RestorePasswordWindow();
            restorePasswordWindow.Activate();
        }

        // No está funcionando como se espera, salta al else aún estando el username y password correctos
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (ValidateCredentials(username, password))
            {
                _adminSessionService.Login(username);

                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Error de Inicio de Sesión",
                    Content = "Usuario o contraseña incorrectos",
                    CloseButtonText = "Ok"
                };
                dialog.XamlRoot = this.XamlRoot;
                await dialog.ShowAsync();
            }
        }

        private bool ValidateCredentials(string username, string password)
        {
            var admin = _context.Admin.SingleOrDefault(a => a.User == username);

            if (admin == null)
            {
                return false; 
            }    

            return VerifyPasswordWithSalt(password, admin.Password, admin.Salt);
        }

        private bool VerifyPasswordWithSalt(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = deriveBytes.GetBytes(32);
                string hash = Convert.ToBase64String(hashBytes);
                return hash == storedHash;
            }
        }
        public (string Hash, string Salt) HashPasswordWithSalt(string password)
        {
            // Generar una sal aleatoria
            byte[] saltBytes = new byte[16];
            RandomNumberGenerator.Fill(saltBytes);  // Generar bytes de sal de manera segura
            string salt = Convert.ToBase64String(saltBytes);

            // Generar el hash con PBKDF2
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = deriveBytes.GetBytes(32); // Genera un hash de 256 bits (32 bytes)
                string hash = Convert.ToBase64String(hashBytes);
                return (hash, salt); // Devolver el hash y la sal generados
            }
        }
    }
}

using System;
using Microsoft.UI.Xaml;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using Assistance.Services;
using Microsoft.Extensions.DependencyInjection;
using Assistance.Models;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Assistance.Views
{
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

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await ShowErrorDialog("Por favor ingresa un usuario y una contraseña.");
                return;
            }

            var admin = _context.Admin.SingleOrDefault(a => a.User == username);

            if (admin != null && ValidateCredentials(password, admin))
            {
                _adminSessionService.Login(username);
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                await ShowErrorDialog("Usuario o contraseña incorrectos.");
            }
        }

        private bool ValidateCredentials(string password, Admin admin)
        {
            return VerifyPasswordWithSalt(password, admin.Password, admin.Salt);
        }

        private bool VerifyPasswordWithSalt(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);  // Convertir el salt desde Base64
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = deriveBytes.GetBytes(32); // Generar el hash
                string hash = Convert.ToBase64String(hashBytes); // Convertir a Base64
                return hash == storedHash; // Comparar con el hash almacenado
            }
        }


        private async Task ShowErrorDialog(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Error de Inicio de Sesión",
                Content = message,
                CloseButtonText = "Ok"
            };
            dialog.XamlRoot = this.XamlRoot;
            await dialog.ShowAsync();
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            var restorePasswordWindow = new RestorePasswordWindow();
            restorePasswordWindow.Activate();
        }
    }
}

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

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Validar credenciales (esta lógica puede conectarse a una base de datos o API)
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

            return VerifyPassword(password, admin.Password);
        }

        public string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            string hashOfInput = HashPassword(password);
            return hashOfInput == storedHash;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Assistance.Views;
using Assistance.Services;
using Microsoft.Extensions.DependencyInjection;
using Assistance.Models;
using System.Security.Cryptography;
using System.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Assistance
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

            // Inyectar el servicio de sesion
            _adminSessionService = ((App)Application.Current).Services.GetRequiredService<AdminSessionService>();

            // Inyectar el contexto de bd
            _context = ((App)Application.Current).Services.GetRequiredService<AssistanceDbContext>();
        }

        // Lógica básica para iniciar sesión
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Validar credenciales (esta lógica puede conectarse a una base de datos o API)
            if (ValidateCredentials(username, password))
            {
                // Iniciar sesion usando el servicio
                _adminSessionService.Login(username);

                // Si el login es exitoso, navega a la página principal
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                // Si el login falla, muestra un mensaje de error
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

        // Método para validar credenciales (puedes agregar lógica para verificar desde una base de datos)
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

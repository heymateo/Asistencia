using Assistance.Models;
using Microsoft.UI.Xaml;
using System.Text;
using System;
using System.Security.Cryptography;
using Microsoft.UI.Xaml.Controls;
using System.Linq;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using WinRT.Interop;
using System.Text.RegularExpressions;

namespace Assistance.Views
{
    public sealed partial class CreateAdminWindow : Window
    {
        public CreateAdminWindow()
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            SetWindowSizeAndPosition(600, 400);
        }

        private void SetWindowSizeAndPosition(int width, int height)
        {
            // Obtener el manejador de la ventana nativa
            var hwnd = WindowNative.GetWindowHandle(this);

            // Obtener el AppWindow asociado
            var appWindow = AppWindow.GetFromWindowId(Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd));

            // Obtener el área de trabajo disponible en la pantalla principal
            var displayArea = DisplayArea.GetFromWindowId(appWindow.Id, DisplayAreaFallback.Primary);
            var workArea = displayArea.WorkArea;

            // Calcular las coordenadas para centrar la ventana
            int centerX = (workArea.Width - width) / 2;
            int centerY = (workArea.Height - height) / 2;

            // Establecer la posición y tamaño fijo de la ventana
            var fixedSize = new SizeInt32(width, height);
            appWindow.Resize(fixedSize);
            appWindow.Move(new PointInt32(centerX, centerY));

            // Asegurar que la ventana no sea redimensionable ni maximizable
            var presenter = appWindow.Presenter as OverlappedPresenter;
            if (presenter != null)
            {
                presenter.IsResizable = false; // Evita la redimensión
                presenter.IsMaximizable = false; // Evita la maximización
            }

            // Asegurar que el tipo de presentación sea el adecuado (estilo estándar)
            appWindow.SetPresenter(AppWindowPresenterKind.Default);
        }

        private async void CreateAdminButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Todos los campos son obligatorios.",
                    CloseButtonText = "OK"
                };
                dialog.XamlRoot = this.Content.XamlRoot; // Aquí se asigna el XamlRoot del contenedor actual.
                await dialog.ShowAsync();
                return;
            }

            using (var dbContext = new AssistanceDbContext())
            {
                try
                {
                    // Verificar si el correo electrónico ya está registrado
                    var existingAdmin = dbContext.Admin.FirstOrDefault(admin => admin.Email == email);
                    if (existingAdmin != null)
                    {
                        var dialog = new ContentDialog
                        {
                            Title = "Error",
                            Content = "El correo electrónico ya está registrado. Use uno diferente.",
                            CloseButtonText = "OK"
                        };
                        dialog.XamlRoot = this.Content.XamlRoot;
                        await dialog.ShowAsync();
                        return;
                    }
                    // Verificar si el username ya está en uso
                    var existingUser = dbContext.Admin.FirstOrDefault(admin => admin.User == username);
                    if (existingUser != null)
                    {
                        var dialog = new ContentDialog
                        {
                            Title = "Error",
                            Content = "El nombre de usuario ya está en uso. Use uno diferente.",
                            CloseButtonText = "OK"
                        };
                        dialog.XamlRoot = this.Content.XamlRoot;
                        await dialog.ShowAsync();
                        return;
                    }
                    // Verificar el formato del correo
                    var emailString = email;
                    if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    {
                        var dialog = new ContentDialog
                        {
                            Title = "Error",
                            Content = "El correo electrónico no tiene un formato válido.",
                            CloseButtonText = "OK"
                        };
                        dialog.XamlRoot = this.Content.XamlRoot;
                        await dialog.ShowAsync();
                        return;
                    }

                    // Genera el hash y la sal para la contraseña
                    var (hashedPassword, salt) = HashPasswordWithSalt(password);

                    // Crear una nueva instancia de Admin
                    var newAdmin = new Admin
                    {
                        User = username,
                        Email = email,
                        Password = hashedPassword,
                        Salt = salt
                    };

                    dbContext.Admin.Add(newAdmin);
                    dbContext.SaveChanges();

                    var successDialog = new ContentDialog
                    {
                        Title = "Éxito",
                        Content = "Administrador creado con éxito.",
                        CloseButtonText = "OK"
                    };
                    successDialog.XamlRoot = this.Content.XamlRoot; // Asignación correcta del XamlRoot.
                    await successDialog.ShowAsync();

                    this.Close();
                }
                catch (Exception ex)
                {
                    var errorDialog = new ContentDialog
                    {
                        Title = "Éxito",
                        Content = $"Ocurrió un error: {ex.Message}",
                        CloseButtonText = "Aceptar",
                        DefaultButton = ContentDialogButton.Close
                    };
                    errorDialog.XamlRoot = this.Content.XamlRoot; // Asignación correcta del XamlRoot.
                    await errorDialog.ShowAsync();
                }
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

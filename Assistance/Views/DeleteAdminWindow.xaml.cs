using Assistance.Models;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Linq;
using Windows.Graphics;
using WinRT.Interop;

namespace Assistance.Views
{
    public sealed partial class DeleteAdminWindow : Window
    {
        public DeleteAdminWindow()
        {
            this.InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            SetWindowSizeAndPosition(600, 300);
        }

        private void SetWindowSizeAndPosition(int width, int height)
        {
            // Obtener el manejador de la ventana nativa
            var hwnd = WindowNative.GetWindowHandle(this);

            // Obtener el AppWindow asociado
            var appWindow = AppWindow.GetFromWindowId(Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd));

            // Configurar el tamaño de la ventana
            var windowSize = new SizeInt32(width, height);
            appWindow.Resize(windowSize);

            // Obtener el área de trabajo disponible en la pantalla principal
            var displayArea = DisplayArea.GetFromWindowId(appWindow.Id, DisplayAreaFallback.Primary);
            var workArea = displayArea.WorkArea;

            // Calcular las coordenadas para centrar la ventana
            int centerX = (workArea.Width - width) / 2;
            int centerY = (workArea.Height - height) / 2;

            // Establecer la posición inicial de la ventana
            appWindow.Move(new PointInt32(centerX, centerY));

            // Establecer tamaño fijo de la ventana
            var fixedSize = new SizeInt32(600, 300); // Ancho y alto en píxeles
            appWindow.Resize(fixedSize);

            // Evitar redimensionar
            appWindow.SetPresenter(AppWindowPresenterKind.Default); // Asegura un estilo estándar sin opciones de redimensionamiento

            // Asegurar que el usuario no puede maximizar o minimizar
            var presenter = appWindow.Presenter as OverlappedPresenter;
            if (presenter != null)
            {
                presenter.IsResizable = false;
                presenter.IsMaximizable = false;
            }
        }

        private void DeleteAdminButton_Click(object sender, RoutedEventArgs e)
        {
            string usernameToSearch = SearchAdminTextBox.Text.Trim();

            using (var dbContext = new AssistanceDbContext())
            {
                try
                {
                    var adminToDelete = dbContext.Admin.FirstOrDefault(a => a.User == usernameToSearch);

                    if (adminToDelete != null)
                    {
                        dbContext.Admin.Remove(adminToDelete);
                        dbContext.SaveChanges();
                        SearchResultTextBlock.Text = "Administrador eliminado con éxito";
                    }
                    else
                    {
                        SearchResultTextBlock.Text = "No se encontró el administrador.";
                    }
                }
                catch (Exception ex)
                {
                    SearchResultTextBlock.Text = $"Error al buscar: {ex.Message}";
                }
            }
        }

        private void SearchAdminButton_Click(object sender, RoutedEventArgs e)
        {
            string username = SearchAdminTextBox.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                SearchResultTextBlock.Text = "Por favor, ingrese un nombre de usuario.";
                return;
            }

            using (var dbContext = new AssistanceDbContext())
            {
                try
                {
                    var admin = dbContext.Admin.FirstOrDefault(a => a.User == username);

                    if (admin != null)
                    {
                        SearchResultTextBlock.Text = $"Nombre: {admin.User}\n" +
                                                     $"Correo: {admin.Email}";
                    }
                    else
                    {
                        SearchResultTextBlock.Text = "No se encontró ningún administrador con ese nombre de usuario.";
                    }
                }
                catch (Exception ex)
                {
                    SearchResultTextBlock.Text = $"Error al buscar: {ex.Message}";
                }
            }
        }
    }
}

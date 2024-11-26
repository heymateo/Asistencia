using Assistance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Assistance.Views
{
    public sealed partial class AdminPage : Page
    {
        public ObservableCollection<Admin> Admin { get; set; }
        private const int PageSize = 3;
        private int CurrentPage = 1;
        private string SelectedSection = null;
        public AdminPage()
        {
            this.InitializeComponent();
            Admin = new ObservableCollection<Admin>();
        }

        private async Task LoadAdmins(int pageNumber = 1)
        {
            using (var dbContext = new AssistanceDbContext())
            {
                try
                {
                    var adminQuery = dbContext.Admin.AsQueryable();

                    var totalAdmins = await adminQuery.CountAsync();

                    var totalPages = (int)Math.Ceiling((double)totalAdmins / PageSize);

                    // Paginacion
                    var admins = await adminQuery
                        .Skip((pageNumber - 1) * PageSize)
                        .Take(PageSize)
                        .ToListAsync();

                    // Limpiar y volver a llenar la colección Observable
                    Admin.Clear();
                    foreach (var admin in admins)
                    {
                        Admin.Add(admin);
                    }

                    // Habilitar o deshabilitar los botones de paginacion
                    PreviousPageButton.IsEnabled = pageNumber > 1;
                    NextPageButton.IsEnabled = pageNumber < totalPages;
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

        private async void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 0)
            {
                CurrentPage++;
                await LoadAdmins(CurrentPage);
            }
        }

        private async void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1) // Evitar decremento por debajo de 1
            {
                CurrentPage--;
                await LoadAdmins(CurrentPage);
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Cargar la primera página de admins por defecto
            await LoadAdmins(CurrentPage);
        }

        // Prueba de transicion de paginas, devuelve null por lo que no funciona.
        private void CreateAdmin_Click(object sender, RoutedEventArgs e)
        {
            var rootFrame = (Frame)Window.Current.Content;
            rootFrame.Navigate(typeof(HomePage));
        }

        // Este funciona
        private void DeleteAdmin_Click(object sender, RoutedEventArgs e)
        {
            var deleteAdminWindow = new DeleteAdminWindow();
            deleteAdminWindow.Activate();
        }


    }
}

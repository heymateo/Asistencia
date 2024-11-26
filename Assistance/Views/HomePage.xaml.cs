using Assistance.Models;
using Assistance.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Assistance.Views
{
    public sealed partial class HomePage : Page
    {
        private readonly AssistanceDbContext _context;
        public HomePage()
        {
            this.InitializeComponent();
            _context = new AssistanceDbContext();
        }
        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            LoadSchoolInfo();
        }

        private void LoadSchoolInfo()
        {
            var schoolInfo = _context.Centro_Educativo.FirstOrDefault();

            if (schoolInfo != null)
            {
                SchoolNameText.Text = schoolInfo.Nombre_Centro;
                SchoolAddressText.Text = $"Dirección: {schoolInfo.Direccion}";
                SchoolPhoneText.Text = $"Teléfono: {schoolInfo.Telefono}";

                if (!string.IsNullOrEmpty(schoolInfo.Logo))
                {
                    SchoolLogo.Source = new BitmapImage(new Uri(schoolInfo.Logo));
                }
            }
            else
            {
                SchoolNameText.Text = "Centro Educativo no encontrado";
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var adminSessionService = ((App)Application.Current).Services.GetRequiredService<AdminSessionService>();

            adminSessionService.Logout();
            Frame.Navigate(typeof(LoginPage));
        }
    }
}

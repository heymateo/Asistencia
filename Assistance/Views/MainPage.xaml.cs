using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Assistance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            // Navigate to the default page
            ContentFrame.Navigate(typeof(HomePage));
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(HomePage))
                NavView.SelectedItem = NavView.MenuItems[0];
            else if (e.SourcePageType == typeof(StudentsPage))
                NavView.SelectedItem = NavView.MenuItems[1];
            else if (e.SourcePageType == typeof(SettingsPage))
                NavView.SelectedItem = NavView.MenuItems[2];
        }

        //private void LogoutButton_Click(object sender, RoutedEventArgs e)
        //{
        //    // Lógica para cerrar sesión
        //    // Por ejemplo, navegar a la página de inicio de sesión
        //    var rootFrame = Window.Current.Content as Frame;
        //    if (rootFrame != null)
        //    {
        //        rootFrame.Navigate(typeof(LoginPage));
        //    }
        //}

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = args.SelectedItem as NavigationViewItem;
            if (selectedItem != null)
            {
                var tag = selectedItem.Tag.ToString();
                switch (tag)
                {
                    case "inicio":
                        ContentFrame.Navigate(typeof(HomePage));
                        break;
                    case "estudiantes":
                        ContentFrame.Navigate(typeof(StudentsPage));
                        break;
                    case "configuracion":
                        ContentFrame.Navigate(typeof(SettingsPage));
                        break;
                }
            }
        }

    }
}

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Assistance.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(HomePage));
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(HomePage))
                NavView.SelectedItem = NavView.MenuItems[0];
            else if (e.SourcePageType == typeof(AdminPage))
                NavView.SelectedItem = NavView.MenuItems[1];
            else if (e.SourcePageType == typeof(StudentsPage))
                NavView.SelectedItem = NavView.MenuItems[2];
            else if (e.SourcePageType == typeof(SettingsPage))
                NavView.SelectedItem = NavView.MenuItems[3];
        }

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
                    case "administradores":
                        ContentFrame.Navigate(typeof(AdminPage)); 
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

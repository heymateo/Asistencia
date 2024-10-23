using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Assistance
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            ExtendsContentIntoTitleBar = true;  // enable custom titlebar
            SetTitleBar(AppTitleBar);      // set user ui element as titlebar

        }

        public static explicit operator MainWindow(UIElement v)
        {
            throw new NotImplementedException();
        }
    }
}

using Microsoft.UI.Xaml;

namespace Assistance
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            ExtendsContentIntoTitleBar = true;  
            SetTitleBar(AppTitleBar);
        }

    }
}

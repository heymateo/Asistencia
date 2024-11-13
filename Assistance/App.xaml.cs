using Assistance.Models;
using Assistance.Services;
using Assistance.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppLifecycle;
using System;
using Windows.ApplicationModel.Activation;
using Microsoft.UI.Dispatching;
using System.Threading.Tasks;

namespace Assistance
{
    public partial class App : Application
    {
        private static DispatcherQueue _dispatcherQueue;
        public static MainWindow MainWindow = new();
        public static RestorePasswordWindow RestorePasswordWindow = new RestorePasswordWindow();

        public IServiceProvider Services { get; private set; }

        public App()
        {
            this.InitializeComponent();
            ConfigureServices();
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddDbContext<AssistanceDbContext>();
            services.AddScoped<CentroEducativoService>();
            services.AddScoped<AdminSessionService>();
            Services = services.BuildServiceProvider();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            if (MainWindow == null)
            {
                MainWindow = new MainWindow();
            }

            var frame = new Frame();
            frame.Navigate(typeof(LoginPage));
            MainWindow.Content = frame;
            MainWindow.Activate();

            // Verificar si la aplicación se activa mediante un protocolo
            var appInstance = AppInstance.GetCurrent();
            var activationArgs = appInstance.GetActivatedEventArgs();

            if (activationArgs.Kind == ExtendedActivationKind.Protocol)
            {
                var protocolArgs = (ProtocolActivatedEventArgs)activationArgs.Data;
                HandleProtocolActivation(protocolArgs.Uri);
            }

        }
        private static void HandleProtocolActivation(Uri uri)
        {
            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                await Task.Yield();

                if (uri.ToString().StartsWith("assistanceapp://resetpassword"))
                {
                    // Extraer el token de la URL
                    var queryParams = new Uri(uri.ToString()).Query;
                    var token = System.Web.HttpUtility.ParseQueryString(queryParams).Get("token");

                    var frame = RestorePasswordWindow.Content as Frame;
                    if (frame == null)
                    {
                        frame = new Frame();
                        RestorePasswordWindow.Content = frame;
                    }

                    _dispatcherQueue.TryEnqueue(() =>
                    {
                        frame.Navigate(typeof(RestorePasswordWindow), token);
                        RestorePasswordWindow.Activate();
                    });
                    await Task.CompletedTask;

                }
            });

        }

    }
}

using Assistance.Models;
using Assistance.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Reflection.PortableExecutable;

namespace Assistance.Views
{
    public sealed partial class RestorePasswordWindow : Window
    {
        private readonly EmailValidationService _emailValidationService;
        private readonly AssistanceDbContext _context;

        public RestorePasswordWindow()
        {
            this.InitializeComponent();
            _emailValidationService = new EmailValidationService();
            _context = new AssistanceDbContext();
        }

        private async void SendResetLinkButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;

            // Validar el formato del correo electrónico
            if (!_emailValidationService.IsValidEmail(email))
            {
                // Mostrar mensaje de error si el correo no tiene un formato válido
                ConfirmationMessage.Text = "Por favor, ingresa un correo electrónico válido.";
                ConfirmationMessage.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                ConfirmationMessage.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                // Configure API key authorization: api-key
                Configuration.Default.AddApiKey("api-key", "YOUR_API_KEY_HERE");

                var apiInstance = new TransactionalEmailsApi();

                string SenderName = "App Asistencia";
                string SenderEmail = "email19291929@gmail.com";
                SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);

                string ToEmail = EmailTextBox.Text;
                SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, null);
                List<SendSmtpEmailTo> toList = new List<SendSmtpEmailTo> { smtpEmailTo};

                string HtmlContent = "<html><body><h1>This is my first transactional email {{params.parameter}}</h1></body></html>";

                string Subject = "Asunto del correo de restablecimiento";

                try
                {
                    var sendSmtpEmail = new SendSmtpEmail(
                    sender: Email,
                    to: toList,
                    subject: Subject,  // Aquí agregamos el asunto obligatorio
                    htmlContent: HtmlContent);

                    CreateSmtpEmail result = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
                    Debug.WriteLine(result.ToJson());
                    Console.WriteLine(result.ToJson());
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                ConfirmationMessage.Text = "Error al enviar el enlace de restablecimiento: " + ex.Message;
                ConfirmationMessage.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                ConfirmationMessage.Visibility = Visibility.Visible;
            }
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            // Cerrar la ventana actual
            this.Close();
        }
    }
}

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
using System.Linq;
using Microsoft.UI.Dispatching;

namespace Assistance.Views
{
    public sealed partial class RestorePasswordWindow : Window
    {
        private readonly EmailValidationService _emailValidationService;
        private readonly PasswordResetTokenService _passwordResetToken;
        private readonly AssistanceDbContext _context;
        private DispatcherQueue _dispatcherQueue;

        public RestorePasswordWindow()
        {
            this.InitializeComponent();
            _emailValidationService = new EmailValidationService();
            _passwordResetToken = new PasswordResetTokenService();
            _context = new AssistanceDbContext();
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        }

        private async void SendResetLinkButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;

            if (!_emailValidationService.IsValidEmail(email))
            {
                UpdateUI(() =>
                {
                    ConfirmationMessage.Text = "Por favor, ingresa un correo electrónico válido.";
                    ConfirmationMessage.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                    ConfirmationMessage.Visibility = Visibility.Visible;
                });
                return;
            }

            var admin = _context.Admin.FirstOrDefault(a => a.Email == email);
            if (admin == null)
            {
                UpdateUI(() =>
                {
                    ConfirmationMessage.Text = "Usuario no encontrado.";
                    ConfirmationMessage.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                    ConfirmationMessage.Visibility = Visibility.Visible;
                });
                return;
            }

            var token = _passwordResetToken.GeneratePasswordResetToken();
            admin.PasswordResetToken = token;
            admin.PasswordResetTokenExpiry = DateTime.Now.AddMinutes(15);
            _context.SaveChanges();

            try
            {
                Configuration.Default.AddApiKey("api-key", "YOUR_API_KEY");

                var apiInstance = new TransactionalEmailsApi();

                string SenderName = "App Asistencia";
                string SenderEmail = "email19291929@gmail.com";
                SendSmtpEmailSender Email = new SendSmtpEmailSender(SenderName, SenderEmail);

                string ToEmail = EmailTextBox.Text;
                SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(ToEmail, null);
                List<SendSmtpEmailTo> toList = new List<SendSmtpEmailTo> { smtpEmailTo };

                string resetLink = $"assistanceapp://resetpassword?token={admin.PasswordResetToken}";


                string HtmlContent = $"<html><body><h1>Restablecer Contraseña</h1><p>Haz clic <a href='{resetLink}'>aquí</a> para restablecer tu contraseña.</p></body></html>";

                string Subject = "Asunto del correo de restablecimiento";

                try
                {
                    var sendSmtpEmail = new SendSmtpEmail(
                    sender: Email,
                    to: toList,
                    subject: Subject,
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
                UpdateUI(() =>
                {
                    ConfirmationMessage.Text = "Error al enviar el enlace de restablecimiento: " + ex.Message;
                    ConfirmationMessage.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                    ConfirmationMessage.Visibility = Visibility.Visible;
                });
            }
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateUI(Action action)
        {
            _dispatcherQueue.TryEnqueue(() => action());
        }
    }
}

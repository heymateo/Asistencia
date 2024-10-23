using Assistance.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Models;
using System;
using System.Runtime.InteropServices;
using Windows.Storage.Pickers;

namespace Assistance.Views
{
    public sealed partial class SettingsPage : Page
    {
        private readonly CentroEducativoService _centroEducativoService;

        public SettingsPage()
        {
            this.InitializeComponent();
            // Resolve the service from the service provider
            _centroEducativoService = ((App)Application.Current).Services.GetRequiredService<CentroEducativoService>();

            CheckCentroEducativoExistence();
        }

        private async void SaveCentroEducativo(Centro_Educativo centro)
        {
            try
            {
                var centroExistente = await _centroEducativoService.ObtenerCentroEducativoAsync();

                if (centroExistente != null)
                {
                    // Actualizar el centro educativo existente
                    centroExistente.Nombre_Centro = centro.Nombre_Centro;
                    centroExistente.Tipo_Institucion = centro.Tipo_Institucion;
                    centroExistente.Direccion = centro.Direccion;
                    centroExistente.Telefono = centro.Telefono;
                    centroExistente.Correo = centro.Correo;
                    centroExistente.Descripcion = centro.Descripcion;
                    centroExistente.Logo = centro.Logo;

                    await _centroEducativoService.ActualizarCentroEducativoAsync(centroExistente);
                    SuccessMessageTextBlock.Text = "Centro educativo actualizado con éxito.";
                }
                else
                {
                    // Guardar nuevo centro educativo
                    await _centroEducativoService.GuardarCentroEducativoAsync(centro);
                    SuccessMessageTextBlock.Text = "Centro educativo guardado con éxito.";
                }

                SuccessMessageTextBlock.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
                SuccessMessageTextBlock.Visibility = Visibility.Visible;

            }
            catch (InvalidOperationException ex)
            {
                SuccessMessageTextBlock.Text = ex.Message;
                SuccessMessageTextBlock.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                SuccessMessageTextBlock.Visibility = Visibility.Visible;
            }
        }

        private async void CheckCentroEducativoExistence()
        {
            var centro = await _centroEducativoService.ObtenerCentroEducativoAsync();

            if (centro != null)
            {
                EditSettingsButton.Visibility = Visibility.Visible;
                // Deshabilitar el botón de guardar
                SaveSettingsButton.IsEnabled = false;

                // Opcional: deshabilitar otros campos del formulario
                SchoolNameTextBox.IsEnabled = false;
                SchoolTypeTextBox.IsEnabled = false;
                SchoolAddressTextBox.IsEnabled = false;
                SchoolPhoneTextBox.IsEnabled = false;
                SchoolEmailTextBox.IsEnabled = false;
                SchoolDescriptionTextBox.IsEnabled = false;
                PickAPhotoButton.IsEnabled = false;

                // Llenar los campos de texto con los datos obtenidos del centro educativo
                SchoolNameTextBox.Text = centro.Nombre_Centro;
                SchoolTypeTextBox.Text = centro.Tipo_Institucion;
                SchoolAddressTextBox.Text = centro.Direccion;
                SchoolPhoneTextBox.Text = centro.Telefono;
                SchoolEmailTextBox.Text = centro.Correo;
                SchoolDescriptionTextBox.Text = centro.Descripcion;
                PickAPhotoOutputTextBlock.Text = centro.Logo;

            }
            else
            {
                // Asegurarse de que el botón de guardar esté habilitado si no existe un centro
                SaveSettingsButton.IsEnabled = true;

                // Opcional: habilitar los campos del formulario si se desea permitir la edición
                SchoolNameTextBox.IsEnabled = true;
                SchoolTypeTextBox.IsEnabled = true;
                SchoolAddressTextBox.IsEnabled = true;
                SchoolPhoneTextBox.IsEnabled = true;
                SchoolEmailTextBox.IsEnabled = true;
                SchoolDescriptionTextBox.IsEnabled = true;
                PickAPhotoButton.IsEnabled = true;
            }
        }
        private void EditSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Habilitar todos los campos para que se puedan editar
            SchoolNameTextBox.IsEnabled = true;
            SchoolTypeTextBox.IsEnabled = true;
            SchoolAddressTextBox.IsEnabled = true;
            SchoolPhoneTextBox.IsEnabled = true;
            SchoolEmailTextBox.IsEnabled = true;
            SchoolDescriptionTextBox.IsEnabled = true;
            PickAPhotoButton.IsEnabled = true;

            // Habilitar el botón de guardar y deshabilitar el botón de "Editar"
            SaveSettingsButton.IsEnabled = true;
            EditSettingsButton.IsEnabled = false;

            // Cambiar el texto del botón de guardar para indicar que ahora es una actualización
            SaveSettingsButton.Content = "Actualizar";
        }
        private async void PickAPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            PickAPhotoOutputTextBlock.Text = "";

            var filePicker = new FileOpenPicker();
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".png");

            var file = await filePicker.PickSingleFileAsync();
            PickAPhotoOutputTextBlock.Text = file != null ? file.Path : "Ningún archivo seleccionado";
        }

        [ComImport]
        [Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IInitializeWithWindow
        {
            void Initialize(IntPtr hwnd);
        }
        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
        internal interface IWindowNative
        {
            IntPtr WindowHandle { get; }
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            string schoolName = SchoolNameTextBox.Text;
            string schoolAddress = SchoolAddressTextBox.Text;
            string schoolType = SchoolTypeTextBox.Text;
            string schoolPhone = SchoolPhoneTextBox.Text;
            string schoolEmail = SchoolEmailTextBox.Text;
            string schoolDescription = SchoolDescriptionTextBox.Text;
            string schoolLogo = PickAPhotoOutputTextBlock.Text;

            var config = new Centro_Educativo
            {
                Nombre_Centro = schoolName,
                Tipo_Institucion = schoolType,
                Direccion = schoolAddress,
                Telefono = schoolPhone,
                Correo = schoolEmail,
                Descripcion = schoolDescription,
                Logo = schoolLogo
            };
             SaveCentroEducativo(config);

            // Deshabilitar el botón de "Actualizar" después de guardar
            SaveSettingsButton.IsEnabled = false;

            // Opcional: Deshabilitar los campos del formulario después de actualizar
            SchoolNameTextBox.IsEnabled = false;
            SchoolTypeTextBox.IsEnabled = false;
            SchoolAddressTextBox.IsEnabled = false;
            SchoolPhoneTextBox.IsEnabled = false;
            SchoolEmailTextBox.IsEnabled = false;
            SchoolDescriptionTextBox.IsEnabled = false;
            PickAPhotoButton.IsEnabled = false;

            // Habilitar nuevamente el botón de "Editar"
            EditSettingsButton.IsEnabled = true;
        }
    }
}

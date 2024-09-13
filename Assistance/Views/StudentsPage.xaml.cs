using Assistance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace Assistance.Views
{
    public sealed partial class StudentsPage : Page
    {
        private readonly AssistanceDbContext _context;
        public ObservableCollection<Estudiante> Estudiantes { get; set; }

        private const int PageSize = 5; // Número de estudiantes por página
        private int CurrentPage = 1;
        private string SelectedSection = null; // Sección seleccionada

        public StudentsPage()
        {
            this.InitializeComponent(); 
            _context = new AssistanceDbContext(); // Asegúrate de que este contexto esté correctamente configurado
            Estudiantes = new ObservableCollection<Estudiante>();
        }

        private async Task LoadStudents(int pageNumber = 1)
        {
            // Si hay una sección seleccionada, filtrar por esa sección
            var studentsQuery = _context.Estudiante.AsQueryable();

            if (!string.IsNullOrEmpty(SelectedSection))
            {
                studentsQuery = studentsQuery.Where(s => s.Seccion == SelectedSection);
            }

            // Total de estudiantes para calcular la cantidad de páginas
            var totalStudents = await studentsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalStudents / PageSize);

            // Paginación
            var students = await studentsQuery
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            // Limpiar y volver a llenar la colección Observable
            Estudiantes.Clear();
            foreach (var student in students)
            {
                Estudiantes.Add(student);
            }

            // Habilitar o deshabilitar los botones de paginación
            PreviousPageButton.IsEnabled = pageNumber > 1;
            NextPageButton.IsEnabled = pageNumber < totalPages;
        }


        private async void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 0)
            {
                CurrentPage++;
                await LoadStudents(CurrentPage);
            }
        }

        private async void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1) // Evitar decremento por debajo de 1
            {
                CurrentPage--;
                await LoadStudents(CurrentPage);
            }
        }

        private async Task<List<string>> GetSectionsAsync()
        {
            // Consulta para obtener las secciones únicas de los estudiantes
            return await _context.Estudiante
                .Select(s => s.Seccion)
                .Distinct()
                .ToListAsync();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Llama al método que obtiene las secciones desde la base de datos
            var sections = await GetSectionsAsync();

            // Limpia los items existentes en el ComboBox
            SectionComboBox.Items.Clear();

            // Popula el ComboBox con las secciones obtenidas
            foreach (var section in sections)
            {
                SectionComboBox.Items.Add(new ComboBoxItem { Content = section });
            }

            // Cargar la primera página de estudiantes por defecto
            await LoadStudents(CurrentPage);
        }

        private async void SectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obtener el valor seleccionado en el ComboBox
            var selectedComboBoxItem = SectionComboBox.SelectedItem as ComboBoxItem;

            if (selectedComboBoxItem != null)
            {
                // Obtener el texto del ComboBoxItem seleccionado
                SelectedSection = selectedComboBoxItem.Content.ToString();

                // Reiniciar la página actual
                CurrentPage = 1;

                // Filtrar los estudiantes por sección y actualizar el ListView
                await LoadStudents(CurrentPage);
            }
        }
    }
}

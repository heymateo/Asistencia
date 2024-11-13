using System;
using System.ComponentModel.DataAnnotations;

namespace Assistance.Models
{
    public class Registro_Asistencia
    {
        [Key]
        public int Id_Registro { get; set; }
        [Required]
        public int Id_Estudiante { get; set; }
        [Required(ErrorMessage = "Fecha")]
        public DateOnly Fecha { get; set; }
        [Required(ErrorMessage = "Hora")]
        public TimeOnly Hora_Entrada { get; set; }
        [Required]
        public bool Asistio { get; set; }
    }
}

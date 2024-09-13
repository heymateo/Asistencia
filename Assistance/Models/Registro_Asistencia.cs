using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistance.Models
{
    public class Registro_Asistencia
    {
        [Key]
        public int Id_Registro { get; set; }
        public int Id_Estudiante { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora_Entrada { get; set; }
        public bool Asistio { get; set; }
    }
}

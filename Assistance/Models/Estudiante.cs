using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace Models
{
    public class Estudiante
    {
        [Key]
        public int Id_Estudiante { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Nivel { get; set; }
        public string Seccion { get; set; }
        public string Grupo { get; set; }
        public string? Especialidad { get; set; }
        public string? Encargado_Legal { get; set; }
        public string? Telefono_Encargado { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    #nullable enable
    public class Centro_Educativo
    {
        [Key]
        public int Id_Centro { get; set; }
        [Required(ErrorMessage ="Escriba el nombre del Centro Educativo")]
        public string Nombre_Centro { get; set; }
        [Required(ErrorMessage = "Escriba el tipo de Centro Educativo")]
        public string Tipo_Institucion { get; set; }
        [Required(ErrorMessage = "Escriba la dirección del Centro Educativo")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "Escriba el teléfono del Centro Educativo")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "Escriba el correo del Centro Educativo")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "Describa el Centro Educativo")]
        public string Descripcion { get; set; }
        public string? Logo { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Assistance.Models
{
#nullable enable
    public class Centro_Educativo
    {
        [Key]
        public int Id_Centro { get; set; }
        [Required(ErrorMessage = "Escriba el nombre del Centro Educativo")]
        public string Nombre_Centro { get; set; } = string.Empty;
        [Required(ErrorMessage = "Escriba el tipo de Centro Educativo")]
        public string Tipo_Institucion { get; set; } = string.Empty;
        [Required(ErrorMessage = "Escriba la dirección del Centro Educativo")]
        public string Direccion { get; set; } = string.Empty;
        [Required(ErrorMessage = "Escriba el teléfono del Centro Educativo")]
        public string Telefono { get; set; } = string.Empty;
        [Required(ErrorMessage = "Escriba el correo del Centro Educativo")]
        public string Correo { get; set; } = string.Empty;
        [Required(ErrorMessage = "Describa el Centro Educativo")]
        public string Descripcion { get; set; } = string.Empty;
        public string? Logo { get; set; }
    }
}

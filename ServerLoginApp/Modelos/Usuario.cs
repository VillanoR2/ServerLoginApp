using System.ComponentModel.DataAnnotations;

namespace ServerLoginApp.Modelos
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre_Usuario { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Rol { get; set; }
        [Required]
        public string Departamento { get; set; }
        [Required]
        public int Telefono { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string BindAlias { get; set; }
    }
}

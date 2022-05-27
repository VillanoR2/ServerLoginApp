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
        [Required, StringLength(10,ErrorMessage ="El valor para {0} debe tener una longitud exacta de {1}",MinimumLength = 10)]
        public string Telefono { get; set; }
        [Required, StringLength(255, ErrorMessage = "El valor para {0} debe tener una longitud entre {2} y {1}", MinimumLength = 7), EmailAddress( ErrorMessage = "Error el dato ingresado no es un correo valido")]
        public string Email { get; set; }
        [Required]
        public string BindAlias { get; set; }
    }
}

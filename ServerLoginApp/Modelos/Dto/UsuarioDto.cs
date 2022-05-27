namespace ServerLoginApp.Modelos.Dto
{
    public class UsuarioDto
    {

        public int Id { get; set; }
        public string Nombre_Usuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Rol { get; set; }
        public string Departamento { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string BindAlias { get; set; }
    }
}

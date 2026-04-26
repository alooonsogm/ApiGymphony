namespace ApiGymphony.Models
{
    public class UsuarioTokenDTO
    {
        public int IdUsuario { get; set; }
        public string NombreRol { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public string Dni { get; set; }
        public string RutaFoto { get; set; }
    }
}

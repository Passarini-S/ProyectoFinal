namespace SistemaDeGestion.Models
{
    public class Usuario
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }
        public string Mail { get; set; }

        public Usuario() { }

        public Usuario(long id, string nombre, string apellido, string nombreUsuario, string contrasenia, string mail)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            NombreUsuario = nombreUsuario;
            Contrasenia = contrasenia;
            Mail = mail;
        }

    }
}

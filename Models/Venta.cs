namespace SistemaDeGestion.Models
{
    public class Venta
    {
        public long Id { get; set; }
        public string Comentarios { get; set; }
        public long IdUsuario { get; set; }

        public Venta()
        { }

        public Venta(long id, string comentarios, long idUsuario)
        {
            Id = id;
            Comentarios = comentarios;
            IdUsuario = idUsuario;
        }

    }
}

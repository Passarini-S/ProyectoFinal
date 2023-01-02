namespace SistemaDeGestion.Models
{
    public class ProductoVendido
    {
        public double Id { get; set; }
        public int Stock { get; set; }
        public double IdProducto { get; set; }
        public double IdVenta { get; set; }

        public ProductoVendido()
        { }

        public ProductoVendido(double id, int stock, double idproducto, double idventa)
        {
            Id = id;
            Stock = stock;
            IdProducto = idproducto;
            IdVenta = idventa;
        }

    }
}

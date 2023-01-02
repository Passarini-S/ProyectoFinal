using SistemaDeGestion.Models;
using System.Data;
using System.Data.SqlClient;

namespace SistemaDeGestion.Repositories
{
    public class ProductoVendidoRepository
    {
        private SqlConnection conexion;
        private String cadenaConexion = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=passarinis_Compras;" +
            "User Id=passarinis_Compras;" +
            "Password=Lobo55##;";
        public ProductoVendidoRepository()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {

            }
        }
        public List<ProductoVendido> listarProductoVendido()
        {
            var lista = new List<ProductoVendido>();
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM ProductoVendido", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ProductoVendido productoVendido = new ProductoVendido();
                                productoVendido.Id = long.Parse(reader["Id"].ToString());
                                productoVendido.Stock = int.Parse(reader["Stock"].ToString());
                                productoVendido.IdProducto = long.Parse(reader["IdProducto"].ToString());
                                productoVendido.IdVenta = long.Parse(reader["IdVenta"].ToString());

                                //public ProductoVendido(double id, int stock, double idproducto, double idproductoVendido)

                                lista.Add(productoVendido);
                            }

                        }
                    }
                }
                conexion.Close();
            }
            catch
            {
                throw;
            }
            return lista;
        }
        private ProductoVendido obtenerProductoVendidoDesdeReader(SqlDataReader reader)
        {
            ProductoVendido productoVendido = new ProductoVendido();
            productoVendido.Id = long.Parse(reader["Id"].ToString());
            productoVendido.Stock = int.Parse(reader["Stock"].ToString());
            productoVendido.IdProducto = long.Parse(reader["IdProducto"].ToString());
            productoVendido.IdVenta = long.Parse(reader["IdVenta"].ToString());

            return productoVendido;
        }

        public ProductoVendido? obtenerProductoVendido(long id)
        {

            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM productoVendido WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            ProductoVendido productoVendido = obtenerProductoVendidoDesdeReader(reader);
                            return productoVendido;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                conexion.Close();
            }
            catch
            {
                throw;
            }

        }

        public bool eliminarProductoVendido(int id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM productoVendido WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    filasAfectadas = cmd.ExecuteNonQuery();
                }
                conexion.Close();
                return filasAfectadas > 0;
            }
            catch
            {
                throw;
            }
        }

        public void crearProductoVendido(ProductoVendido productoVendido)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                //int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("INSERT INTO productoVendido(Comentarios, IdUsuario) " +
                    "VALUES(@comentarios, @idUsuario)", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = productoVendido.Stock });
                    cmd.Parameters.Add(new SqlParameter("idProducto", SqlDbType.BigInt) { Value = productoVendido.IdProducto });
                    cmd.Parameters.Add(new SqlParameter("idVenta", SqlDbType.BigInt) { Value = productoVendido.IdVenta });
                    cmd.ExecuteNonQuery();
                }
                conexion.Close();
                //return filasAfectadas > 0;
            }
            catch
            {
                throw;
            }
        }

        public ProductoVendido? actualizarProductoVendido(long id, ProductoVendido productoVendidoAActualizar)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                ProductoVendido? productoVendido = obtenerProductoVendido(id);
                if (productoVendido == null)
                { return null; }
                List<string> camposAActualizar = new List<string>();
              
                if (productoVendido.Stock != productoVendidoAActualizar.Stock && productoVendidoAActualizar.Stock > 0)
                {
                    camposAActualizar.Add("stock = @stock");
                    productoVendido.Stock = productoVendidoAActualizar.Stock;
                }
                if (productoVendido.IdProducto != productoVendidoAActualizar.IdProducto && productoVendidoAActualizar.IdProducto > 0)
                {
                    camposAActualizar.Add("idProducto = @idProducto");
                    productoVendido.IdProducto = productoVendidoAActualizar.IdProducto;
                }
                if (productoVendido.IdVenta != productoVendidoAActualizar.IdVenta && productoVendidoAActualizar.IdVenta > 0)
                {
                    camposAActualizar.Add("idVenta = @idVenta");
                    productoVendido.IdVenta = productoVendidoAActualizar.IdVenta;
                }

                if (camposAActualizar.Count == 0)
                {
                    throw new Exception("No hay campos para actualizar");
                }
                using (SqlCommand cmd = new SqlCommand($"UPDATE ProductoVendido SET {string.Join(", ", camposAActualizar)} WHERE id = @id", conexion))
                {
                   
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = productoVendidoAActualizar.Stock });
                    cmd.Parameters.Add(new SqlParameter("idProducto", SqlDbType.BigInt) { Value = productoVendidoAActualizar.IdProducto });
                    cmd.Parameters.Add(new SqlParameter("idVenta", SqlDbType.BigInt) { Value = productoVendidoAActualizar.IdVenta });

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return productoVendido;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}

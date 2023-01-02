using SistemaDeGestion.Models;
using System.Data;
using System.Data.SqlClient;

namespace SistemaDeGestion.Repositories
{
    public class ProductosRepository
    {
        private SqlConnection conexion;
        private String cadenaConexion = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=passarinis_Compras;" +
            "User Id=passarinis_Compras;" +
            "Password=Lobo55##;";

        public ProductosRepository()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {

            }
        }

        public List<Producto> listarProductos()
        {
            List<Producto> lista = new List<Producto>();
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM producto", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Producto producto = obtenerProductoDesdeReader(reader);
                                lista.Add(producto);
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
        public Producto? obtenerProducto(long id)
        {
            
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM producto WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Producto producto = obtenerProductoDesdeReader(reader);
                            return producto;
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

        public bool eliminarProducto(int id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
        try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM producto WHERE id = @id", conexion))
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

        public void crearProducto(Producto producto)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                //int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Producto(Descripciones, Costo, PrecioVenta, Stock, IdUsuario) " +
                    "VALUES(@descripciones, @costo, @precioVenta, @stock, @idUsuario)", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("descripciones", SqlDbType.VarChar) { Value = producto.Descripcion });
                    cmd.Parameters.Add(new SqlParameter("costo", SqlDbType.Float) { Value = producto.Costo });
                    cmd.Parameters.Add(new SqlParameter("precioVenta", SqlDbType.Float) { Value = producto.PrecioVenta });
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = producto.Stock });
                    cmd.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.BigInt) { Value = producto.IdUsuario });
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

        private Producto obtenerProductoDesdeReader(SqlDataReader reader)
        {
            Producto producto = new Producto();
            producto.Id = long.Parse(reader["Id"].ToString());
            producto.Descripcion = reader["Descripciones"].ToString();
            producto.Costo = double.Parse(reader["Costo"].ToString());
            producto.PrecioVenta = double.Parse(reader["PrecioVenta"].ToString());
            producto.Stock = int.Parse(reader["Stock"].ToString());
            return producto;
        }

        public Producto? actualizarProducto(long id, Producto productoAActualizar)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                Producto? producto = obtenerProducto(id);
                if (producto == null)
                { return null; }
                List<string> camposAActualizar = new List<string>();
                if (producto.Descripcion != productoAActualizar.Descripcion && !string.IsNullOrEmpty(productoAActualizar.Descripcion))
                {
                    camposAActualizar.Add("descripciones = @descipcion");
                    producto.Descripcion = productoAActualizar.Descripcion;
                }
                if (producto.Costo != productoAActualizar.Costo && productoAActualizar.Costo > 0)
                {
                    camposAActualizar.Add("costo = @costo");
                    producto.Costo = productoAActualizar.Costo;
                }
                if (producto.PrecioVenta != productoAActualizar.PrecioVenta && productoAActualizar.PrecioVenta > 0)
                {
                    camposAActualizar.Add("precioVenta = @recioVenta");
                    producto.PrecioVenta = productoAActualizar.PrecioVenta;
                }
                if (producto.Stock != productoAActualizar.Stock && productoAActualizar.Stock > 0)
                {
                    camposAActualizar.Add("stock = @stock");
                    producto.Stock = productoAActualizar.Stock;
                }
                if (camposAActualizar.Count == 0)
                {
                    throw new Exception("No hay campos para actualizar");
                }
                using (SqlCommand cmd = new SqlCommand($"UPDATE Producto SET {string.Join(", ", camposAActualizar )} WHERE id = @id", conexion))
                {
                    cmd.Parameters.Add(new SqlParameter("descripcion", SqlDbType.VarChar) { Value = productoAActualizar.Descripcion });
                    cmd.Parameters.Add(new SqlParameter("costo", SqlDbType.Float) { Value = productoAActualizar.Costo });
                    cmd.Parameters.Add(new SqlParameter("precioVenta", SqlDbType.Float) { Value = productoAActualizar.PrecioVenta });
                    cmd.Parameters.Add(new SqlParameter("stock", SqlDbType.Int) { Value = productoAActualizar.Stock });
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return producto;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}

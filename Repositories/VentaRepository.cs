using SistemaDeGestion.Models;
using System.Data;
using System.Data.SqlClient;

namespace SistemaDeGestion.Repositories
{
    public class VentaRepository
    {
        private SqlConnection conexion;
        private String cadenaConexion = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=passarinis_Compras;" +
            "User Id=passarinis_Compras;" +
            "Password=Lobo55##;";
        public VentaRepository()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {

            }
        }
        public List<Venta> listarVenta()
        {
            List<Venta> lista = new List<Venta>();
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Venta", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Venta venta = new Venta();
                                venta.Id = long.Parse(reader["Id"].ToString());
                                venta.Comentarios = reader["Comentarios"].ToString();
                                venta.IdUsuario = long.Parse(reader["IdUsuario"].ToString());

                                lista.Add(venta);
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
        private Venta obtenerVentaDesdeReader(SqlDataReader reader)
        {
            Venta venta = new Venta();
            venta.Id = long.Parse(reader["Id"].ToString());
            venta.Comentarios = reader["Nombre"].ToString();
            venta.IdUsuario = long.Parse(reader["IdUsuario"].ToString());

            return venta;
        }

        public Venta? obtenerVenta(long id)
        {

            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM venta WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Venta venta = obtenerVentaDesdeReader(reader);
                            return venta;
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

        public bool eliminarVenta(int id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM venta WHERE id = @id", conexion))
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

        public void crearVenta(Venta venta)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                //int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("INSERT INTO venta(Comentarios, IdUsuario) " +
                    "VALUES(@comentarios, @idUsuario)", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("comentarios", SqlDbType.VarChar) { Value = venta.Comentarios });
                    cmd.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.BigInt) { Value = venta.IdUsuario });

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

        public Venta? actualizarVenta(long id, Venta ventaAActualizar)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                Venta? venta = obtenerVenta(id);
                if (venta == null)
                { return null; }
                List<string> camposAActualizar = new List<string>();
                if (venta.Comentarios != ventaAActualizar.Comentarios && !string.IsNullOrEmpty(ventaAActualizar.Comentarios))
                {
                    camposAActualizar.Add("comentarios = @comentarios");
                    venta.Comentarios = ventaAActualizar.Comentarios;
                }
                if (venta.IdUsuario != ventaAActualizar.IdUsuario && ventaAActualizar.IdUsuario > 0)
                {
                    camposAActualizar.Add("idusuario = @idusuario");
                    venta.IdUsuario = ventaAActualizar.IdUsuario;
                }

                if (camposAActualizar.Count == 0)
                {
                    throw new Exception("No hay campos para actualizar");
                }
                using (SqlCommand cmd = new SqlCommand($"UPDATE Venta SET {string.Join(", ", camposAActualizar)} WHERE id = @id", conexion))
                {
                    cmd.Parameters.Add(new SqlParameter("comentarios", SqlDbType.VarChar) { Value = ventaAActualizar.Comentarios });
                    cmd.Parameters.Add(new SqlParameter("idUsuario", SqlDbType.BigInt) { Value = ventaAActualizar.IdUsuario });

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return venta;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}

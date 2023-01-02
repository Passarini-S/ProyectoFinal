using SistemaDeGestion.Models;
using System.Data;
using System.Data.SqlClient;

namespace SistemaDeGestion.Repositories
{
    public class UsuarioRepository
    {
        private SqlConnection conexion;
        private String cadenaConexion = "Server=sql.bsite.net\\MSSQL2016;" +
            "Database=passarinis_Compras;" +
            "User Id=passarinis_Compras;" +
            "Password=Lobo55##;";
        public UsuarioRepository()
        {
            try
            {
                conexion = new SqlConnection(cadenaConexion);
            }
            catch (Exception ex)
            {

            }
        }
        public List<Usuario> listarUsuario()
        {
            List<Usuario> lista = new List<Usuario>();
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario", conexion))
                {
                    conexion.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Usuario usuario = new Usuario();
                                usuario.Id = long.Parse(reader["Id"].ToString());
                                usuario.Nombre = reader["Nombre"].ToString();
                                usuario.Apellido = reader["Apellido"].ToString();
                                usuario.NombreUsuario = reader["NombreUsuario"].ToString();
                                usuario.Contrasenia = reader["Contraseña"].ToString();
                                usuario.Mail = reader["Mail"].ToString();
                                lista.Add(usuario);
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

        private Usuario obtenerUsuarioDesdeReader(SqlDataReader reader)
        {
            Usuario usuario = new Usuario();
            usuario.Id = long.Parse(reader["Id"].ToString());
            usuario.Nombre = reader["Nombre"].ToString();
            usuario.Apellido = reader["Apellido"].ToString();
            usuario.NombreUsuario = reader["NombreUsuario"].ToString();
            usuario.Contrasenia = reader["Contrasenia"].ToString();
            usuario.Mail = reader["Mail"].ToString();

            return usuario;
        }

        public Usuario? obtenerUsuario(long id)
        {

            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM usuario WHERE id = @id", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("id", SqlDbType.BigInt) { Value = id });
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Usuario usuario = obtenerUsuarioDesdeReader(reader);
                            return usuario;
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

        public bool eliminarUsuario(int id)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("DELETE FROM usuario WHERE id = @id", conexion))
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

        public void crearUsuario(Usuario usuario)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                //int filasAfectadas = 0;
                using (SqlCommand cmd = new SqlCommand("INSERT INTO usuario(Nombre, Apellido, NombreUsuario, Contraseña, Mail) " +
                    "VALUES(@nombre, @apellido, @nombreUsuario, @contrasenia, @mail)", conexion))
                {
                    conexion.Open();
                    cmd.Parameters.Add(new SqlParameter("nombre", SqlDbType.VarChar) { Value = usuario.Nombre });
                    cmd.Parameters.Add(new SqlParameter("apellido", SqlDbType.VarChar) { Value = usuario.Apellido });
                    cmd.Parameters.Add(new SqlParameter("nombreUsuario", SqlDbType.VarChar) { Value = usuario.NombreUsuario });
                    cmd.Parameters.Add(new SqlParameter("contrasenia", SqlDbType.VarChar) { Value = usuario.Contrasenia });
                    cmd.Parameters.Add(new SqlParameter("mail", SqlDbType.VarChar) { Value = usuario.Mail });
                                
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

        public Usuario? actualizarUsuario(long id, Usuario usuarioAActualizar)
        {
            if (conexion == null)
            {
                throw new Exception("Conexión no establecida");
            }
            try
            {
                Usuario? usuario = obtenerUsuario(id);
                if (usuario == null)
                { return null; }
                List<string> camposAActualizar = new List<string>();
                if (usuario.Nombre != usuarioAActualizar.Nombre && !string.IsNullOrEmpty(usuarioAActualizar.Nombre))
                {
                    camposAActualizar.Add("nombre = @nombre");
                    usuario.Nombre = usuarioAActualizar.Nombre;
                }
                if (usuario.Apellido != usuarioAActualizar.Apellido && !string.IsNullOrEmpty(usuarioAActualizar.Apellido))
                {
                    camposAActualizar.Add("apellido = @apellido");
                    usuario.Apellido = usuarioAActualizar.Apellido;
                }
                if (usuario.NombreUsuario != usuarioAActualizar.NombreUsuario && !string.IsNullOrEmpty(usuarioAActualizar.NombreUsuario))
                {
                    camposAActualizar.Add("nombreUsuario = @nombreUsuario");
                    usuario.NombreUsuario = usuarioAActualizar.NombreUsuario;
                }
                if (usuario.Contrasenia != usuarioAActualizar.Contrasenia && !string.IsNullOrEmpty(usuarioAActualizar.Contrasenia))
                {
                    camposAActualizar.Add("contraseña = @contrasenia");
                    usuario.Contrasenia = usuarioAActualizar.Contrasenia;
                }
                if (usuario.Mail != usuarioAActualizar.Mail && !string.IsNullOrEmpty(usuarioAActualizar.Mail))
                {
                    camposAActualizar.Add("mail = @mail");
                    usuario.Mail = usuarioAActualizar.Mail;
                }


                if (camposAActualizar.Count == 0)
                {
                    throw new Exception("No hay campos para actualizar");
                }
                using (SqlCommand cmd = new SqlCommand($"UPDATE Usuario SET {string.Join(", ", camposAActualizar)} WHERE id = @id", conexion))
                {
                    cmd.Parameters.Add(new SqlParameter("nombre", SqlDbType.VarChar) { Value = usuarioAActualizar.Nombre });
                    cmd.Parameters.Add(new SqlParameter("apellido", SqlDbType.VarChar) { Value = usuarioAActualizar.Apellido });
                    cmd.Parameters.Add(new SqlParameter("nombreUsuario", SqlDbType.VarChar) { Value = usuarioAActualizar.NombreUsuario });
                    cmd.Parameters.Add(new SqlParameter("contrasenia", SqlDbType.VarChar) { Value = usuarioAActualizar.Contrasenia });
                    cmd.Parameters.Add(new SqlParameter("mail", SqlDbType.VarChar) { Value = usuarioAActualizar.Mail });

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    return usuario;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}

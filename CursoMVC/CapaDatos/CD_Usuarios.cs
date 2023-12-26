using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        // Método para retorar una lista de usuarios
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                Console.WriteLine("Iniciando el método Listar");

                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                using (SqlCommand cmd = new SqlCommand("SELECT idUsuario, Nombres, Apellidos, Correo, Clave, Reestablecer, Activo FROM Usuario", oconexion))
                {
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open(); // Abre la conexión

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Usuario
                            {
                                idUsuario = Convert.ToInt32(dr["idUsuario"]),
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Reestablecer = dr["Reestablecer"] == DBNull.Value ? false : Convert.ToBoolean(dr["Reestablecer"]),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }

                    Console.WriteLine("Método Listar completado con éxito");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error de SQL Server (Número " + ex.Number + "): " + ex.Message);

                switch (ex.Number)
                {
                    // Manejar errores específicos de SQL Server
                    case 2601:
                        Console.WriteLine("Error de violación de clave única.");
                        break;
                    // Otros casos según sea necesario
                    default:
                        break;
                }

                lista = new List<Usuario>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error general: " + ex.Message);
                lista = new List<Usuario>();
            }

            return lista;
        }


        ///Este es el metodo para ingresar nuevo usuario llamando a un sp InsertarUsuario
        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                Console.WriteLine("Iniciando el método Registrar");

                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("InsertarUsuario", oconexion);

                    // Parámetros del comando
                    cmd.Parameters.AddWithValue("@Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("@Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("@Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open(); // Abre la conexión

                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (SqlException ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }

            return idautogenerado;
        }


        ///Este es el metodo para Editar nuevo usuario llamando a un sp EditarUsuario
        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                Console.WriteLine("Iniciando el método Registrar");

                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("EditarUsuario", oconexion);

                    // Parámetros del comando
                    cmd.Parameters.AddWithValue("@idUsuario", obj.idUsuario);
                    cmd.Parameters.AddWithValue("@Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("@Correo", obj.Correo);
                    //--  cmd.Parameters.AddWithValue("@Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open(); // Abre la conexión

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (SqlException ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }



        ///Este es el metodo para Eliminar nuevo usuario llamando a un sp EditarUsuario
           public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("EliminarUsuario", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUsuario", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
            }

            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }




        public bool CambiarClave(int idUsuario, string nuevaclave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update usuario set clave = @nuevaclave, reestablecer = 0 where idusuario = @idusuario ", oconexion);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@nuevaclave", nuevaclave);

                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
            }

            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }




        public bool Reestablecer(int idUsuario, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update usuario set clave = @nuevaclave, reestablecer = 1 where idusuario = @idusuario ", oconexion);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@clave", clave);

                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
            }

            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }









    }

}

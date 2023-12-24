using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using CapaEntidad;


namespace CapaDatos
{
    public class CD_Categoria
    {
        public List<Categoria> Listar()
        {
            List<Categoria> lista = new List<Categoria>();

            try
            {
                Console.WriteLine("Iniciando el método Listar");

                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                using (SqlCommand cmd = new SqlCommand("SELECT idCategoria, descripcion, Activo FROM CATEGORIA", oconexion))
                {
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open(); // Abre la conexión

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Categoria
                            {
                                idCategoria = Convert.ToInt32(dr["idCategoria"]),
                                descripcion = dr["descripcion"].ToString(),
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

                lista = new List<Categoria>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error general: " + ex.Message);
                lista = new List<Categoria>();
            }

            return lista;
        }



        ///Este es el metodo para Editar nuevo Categoria llamando a un sp sp_RegistrarCategoria
        public int Registrar(Categoria obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                Console.WriteLine("Iniciando el método sp_RegistrarCategoria");

                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCategoria", oconexion);

                    // Parámetros del comando
                    cmd.Parameters.AddWithValue("@Descripcion", obj.descripcion);
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



        ///Este es el metodo para Editar nuevo Categoria llamando a un sp sp_EditarCategoria
        public bool Editar(Categoria obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                Console.WriteLine("Iniciando el método Registrar");

                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarCategoria", oconexion);

                    // Parámetros del comando
                    cmd.Parameters.AddWithValue("@idCategoria", obj.@idCategoria);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.descripcion);
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


        ///Este es el metodo para Eliminar nuevo Categoria llamando a un sp sp_EliminarCategoria
        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarCategoria", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idCategoria", id);
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

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

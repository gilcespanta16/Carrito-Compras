using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Productos
    {

        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                //Console.WriteLine("Iniciando el método Listar");

                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                //using (SqlCommand cmd = new SqlCommand("SELECT idProducto, descripcion, Activo FROM PRODUCTO", oconexion))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("select p.idProducto, p.Nombre, p.Descripcion,");
                    sb.AppendLine("m.idMarca, m.Descripcion[DesMarca],");
                    sb.AppendLine("c.idCategoria, c.descripcion[DesCategoria],");
                    sb.AppendLine("p.Precio, p.Stock, p.RutaImagen, p.NombreImagen, p.Activo");
                    sb.AppendLine("from PRODUCTO p");
                    sb.AppendLine("inner join MARCA m on m.idMarca = p.idCategoria");
                    sb.AppendLine("inner join CATEGORIA c on c.idCategoria = p.idCategoria;");


                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open(); // Abre la conexión

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Producto()
                            {
                                idProducto = Convert.ToInt32(dr["idProducto"]),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                oMarca = new Marca() { idMarca = Convert.ToInt32(dr["idMarca"]), Descripcion = dr["DesMarca"].ToString() },
                                oCategoria = new Categoria() { idCategoria = Convert.ToInt32(dr["idMarca"]), descripcion = dr["DesCategoria"].ToString() },
                                Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-PE")),
                                Stock = Convert.ToInt32(dr["Stock"]),
                                RutaImagen = dr["RutaImagen"].ToString(),
                                NombreImagen = dr["NombreImagen"].ToString(),
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

                lista = new List<Producto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error general: " + ex.Message);
                lista = new List<Producto>();
            }

            return lista;
        }


        ///Este es el metodo para Registrar nuevo Producto llamando a un sp sp_RegistrarProducto
        public int Registrar(Producto obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                Console.WriteLine("Iniciando el método sp_RegistrarProducto");

                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarProducto", oconexion);

                    // Parámetros del comando
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("@idMarca", obj.oMarca.idMarca);
                    cmd.Parameters.AddWithValue("@idCategoria", obj.oCategoria.idCategoria);
                    cmd.Parameters.AddWithValue("@Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("@Stock", obj.Stock);
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



        ///Este es el metodo para Editar nuevo Producto llamando a un sp sp_EditarProducto
        public bool Editar(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                Console.WriteLine("Iniciando el método Registrar");

                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarProducto", oconexion);

                    // Parámetros del comando
                    cmd.Parameters.AddWithValue("@idProducto", obj.@idProducto);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("@idMarca", obj.oMarca.idMarca);
                    cmd.Parameters.AddWithValue("@idCategoria", obj.oCategoria.idCategoria);
                    cmd.Parameters.AddWithValue("@Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("@Stock", obj.Stock);
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


        ///Este es el metodo para Eliminar nuevo Producto llamando a un sp sp_EliminarProducto
        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarProducto", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idProducto", id);
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;


                    //cmd.ExecuteNonQuery();
                    //resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    //Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }

            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }



        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "UPDATE producto SET RutaImagen = @rutaimagen, NombreImagen = @nombreimagen WHERE idProducto = @idProducto";

                    using (SqlCommand cmd = new SqlCommand(query, oconexion))
                    {
                        cmd.Parameters.AddWithValue("@rutaimagen", obj.RutaImagen);
                        cmd.Parameters.AddWithValue("@nombreimagen", obj.NombreImagen);
                        cmd.Parameters.AddWithValue("@idproducto", obj.idProducto);

                        cmd.CommandType = CommandType.Text;

                        oconexion.Open(); // Abre la conexión

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            resultado = true;
                        }
                        else
                        {
                            Mensaje = "No se pudo actualizar la imagen";
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }
    }
}

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
   public class CD_Reporte
    {
        public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> lista = new List<Reporte>();

            try
            {
                Console.WriteLine("Iniciando el método Listar");

                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                using (SqlCommand cmd = new SqlCommand("sp_ReporteVentas", oconexion))
                {
                    //estos son los parametro que esperan el sp;
                    cmd.Parameters.AddWithValue("fechainicio", fechainicio);
                    cmd.Parameters.AddWithValue("fechafin", fechafin);
                    cmd.Parameters.AddWithValue("idtransaccion", idtransaccion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open(); // Abre la conexión

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Reporte
                            {
                                FechaVenta = dr["FechaVenta"].ToString(),
                                Cliente = dr["Cliente"].ToString(),
                                Producto = dr["Producto"].ToString(),
                                Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-PE")),
                                Cantidad = Convert.ToInt32(dr["Cantidad"].ToString()),
                                Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-PE")),
                                idTransaccion = dr["idTransaccion"].ToString()
                            });
                        }
                    }

                }
            }
            catch 
            {
                lista = new List<Reporte>();
            }

            return lista;
        }







        public Dashboard verDashboard()
        {
            Dashboard objeto = new Dashboard();

            try
            {
                Console.WriteLine("Iniciando el método Listar");

                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                using (SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", oconexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open(); // Abre la conexión

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objeto = new Dashboard()
                            {
                                TotalCliente = Convert.ToInt32(dr["TotalCliente"]),
                                TotalVenta = Convert.ToInt32(dr["TotalVenta"]),
                                TotalProducto = Convert.ToInt32(dr["TotalProducto"]),
                            };
                        }
                    }
                }
            }
            catch
            {
                objeto = new Dashboard();
            }
            return objeto;
        }


    }
}

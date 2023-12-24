using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace CapaPresentacionAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> oLista = new List<Usuario>();
            oLista = new CN_Usuarios().Listar();
            return Json(new { data = oLista, estado = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarUsuarios(Usuario objeto)
        {
            object Resultado;         
            string Mensaje = string.Empty;

            if (objeto.idUsuario == 0)
            {
                Resultado = new CN_Usuarios().Registrar(objeto, out Mensaje);
            }else
            {
                Resultado = new CN_Usuarios().Editar(objeto, out Mensaje);
            }
            return Json(new { resultado = Resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            // Llama al método Eliminar de la capa de negocios
            respuesta = new CN_Usuarios().Eliminar(id, out mensaje);

            //return Json(new { resultado = respuesta, mensaje = mensaje });
            return Json(new { resultado = respuesta, mensaje = mensaje, idUsuario = id }, JsonRequestBehavior.AllowGet);
        }


        //Se crea un metodo GET  con 3 parametros que devuelve y envia un JSOM en una lista
        [HttpGet]
        public JsonResult ListaReporte(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> oLista = new List<Reporte>();

            oLista = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);

        }





        [HttpGet]
        public JsonResult VistaDashboard()
        {
            Dashboard objeto = new CN_Reporte().verDashboard();
            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);

        }


        //Creo un metodo post Exportar venta que reciben 3 paramtros
        [HttpPost]
        public FileResult ExportarVenta(string fechainicio, string fechafin, string idtransaccion) {

            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_Reporte().Ventas(fechainicio, fechafin, idtransaccion);

            DataTable dt = new DataTable();

            dt.Locale = new System.Globalization.CultureInfo("es-PE");
            dt.Columns.Add("FechaVenta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(string));
            dt.Columns.Add("Cantidad", typeof(string));
            dt.Columns.Add("Total", typeof(string));
            dt.Columns.Add("idTransaccion", typeof(string));

            foreach(Reporte rp in oLista)
            {
                dt.Rows.Add(new object[]
                {
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.idTransaccion
                });
            }
            dt.TableName = "Datos";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta" + DateTime.Now.ToString() + ".xlsx");

                }
            }

        }
    }


}

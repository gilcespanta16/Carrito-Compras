using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System.Globalization;
using System.Configuration;
using System.IO;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }

        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////  CATEGORIA  ////////////////////////////////////////////////////////////////
        #region Categoria
        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = new CN_Categoria().Listar();
            return Json(new { data = oLista, estado = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            object Resultado;
            string Mensaje = string.Empty;

            if (objeto.idCategoria == 0)
            {
                Resultado = new CN_Categoria().Registrar(objeto, out Mensaje);
            }
            else
            {
                Resultado = new CN_Categoria().Editar(objeto, out Mensaje);
            }
            return Json(new { resultado = Resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            // Llama al método Eliminar de la capa de negocios
            respuesta = new CN_Categoria().Eliminar(id, out mensaje);

            //return Json(new { resultado = respuesta, mensaje = mensaje });
            return Json(new { resultado = respuesta, mensaje = mensaje, idUsuario = id }, JsonRequestBehavior.AllowGet);


        }
        #endregion


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////  MARCA  ////////////////////////////////////////////////////////////////
        #region Marca

        [HttpGet]
        public JsonResult ListarMarca()
        {
            List<Marca> oLista = new List<Marca>();
            oLista = new CN_Marca().Listar(); // Asegúrate de tener un CN_Marca para manejar la lógica de negocios de Marca
            return Json(new { data = oLista, estado = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            object Resultado;
            string Mensaje = string.Empty;

            if (objeto.idMarca == 0)
            {
                Resultado = new CN_Marca().Registrar(objeto, out Mensaje);
            }
            else
            {
                Resultado = new CN_Marca().Editar(objeto, out Mensaje);
            }
            return Json(new { resultado = Resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            // Llama al método Eliminar de la capa de negocios
            respuesta = new CN_Marca().Eliminar(id, out mensaje);

            //return Json(new { resultado = respuesta, mensaje = mensaje });
            return Json(new { resultado = respuesta, mensaje = mensaje, idUsuario = id }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////  Producto  ////////////////////////////////////////////////////////////////
        #region Producto
        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> oLista = new List<Producto>();
            oLista = new CN_Productos().Listar(); // Asegúrate de tener un CN_Marca para manejar la lógica de negocios de Marca
            return Json(new { data = oLista, estado = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            string Mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guarda_imagen_exito = true;

            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto);

            decimal precio;

            if (decimal.TryParse(oProducto.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out precio))
            {
                oProducto.Precio = precio;
            }
            else
            {
                return Json(new { operacionExitosa = false, mensaje = "El formato del precio debe ser ##.## " }, JsonRequestBehavior.AllowGet);
            }

            if (oProducto.idProducto == 0)
            {
                int idProductoGenerado = new CN_Productos().Registrar(oProducto, out Mensaje);

                if (idProductoGenerado != 0)
                {
                    oProducto.idProducto = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                operacion_exitosa = new CN_Productos().Editar(oProducto, out Mensaje);
            }

            if (operacion_exitosa)
            {
                if (archivoImagen != null)
                {
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oProducto.idProducto.ToString(), extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        guarda_imagen_exito = false;
                    }

                    if (guarda_imagen_exito)
                    {
                        oProducto.RutaImagen = ruta_guardar;
                        oProducto.NombreImagen = nombre_imagen;
                        bool rspta = new CN_Productos().GuardarDatosImagen(oProducto, out Mensaje);
                    }
                    else
                    {
                        Mensaje = "Se guardo el producto pero hubo problemas con la imagen";
                    }
                }
            }

            return Json(new { operacionExitosa = operacion_exitosa, idGenerado = oProducto.idProducto, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Producto oproducto = new CN_Productos().Listar().Where(p => p.idProducto == id).FirstOrDefault();

            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oproducto.RutaImagen, oproducto.NombreImagen), out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oproducto.NombreImagen)
            }, JsonRequestBehavior.AllowGet);
        }





        //[HttpPost]
        //public JsonResult ImagenProducto(int id)
        //{
        //    try
        //    {
        //        // Tu código actual aquí
        //        bool conversion;
        //        Producto oproducto = new CN_Productos().Listar().Where(p => p.idProducto == id).FirstOrDefault();

        //        string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oproducto.RutaImagen, oproducto.NombreImagen), out conversion);

        //        return Json(new
        //        {
        //            conversion = conversion,
        //            textoBase64 = textoBase64,
        //            extension = Path.GetExtension(oproducto.NombreImagen)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            error = ex.Message  // O cualquier otra información de error que desees incluir
        //        });
        //    }
        //}












        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            // Llama al método Eliminar de la capa de negocios
            respuesta = new CN_Productos().Eliminar(id, out mensaje);

            //return Json(new { resultado = respuesta, mensaje = mensaje });
            return Json(new { resultado = respuesta, mensaje = mensaje, idProducto = id }, JsonRequestBehavior.AllowGet);
            //return Json(new { resultado = respuesta, mensaje = mensaje, idProductos = id }, JsonRequestBehavior.DenyGet);

        }

        #endregion


    }
}

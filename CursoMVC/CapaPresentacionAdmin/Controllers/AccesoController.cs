using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            bool cambioClaveExitoso = TempData["CambioClaveExitoso"] != null && (bool)TempData["CambioClaveExitoso"];

            if (cambioClaveExitoso)
            {
                ViewBag.MensajeCambioClave = "Cambio de clave exitoso. Ingrese nuevamente con sus credenciales.";
            }

            // Resto de la lógica para la acción Index

            return View();
        }


        public ActionResult CambiarClave()
        {
            return View();
        }


        public ActionResult Reestablecer()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Usuario oUsuario = new Usuario();
            oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "Correo o contraseña Incorrecta";
                return View();
            }
            else {
                if (oUsuario.Reestablecer)
                {
                    TempData["idUsuario"] = oUsuario.idUsuario;
                    return RedirectToAction("CambiarClave");

                }

                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }

        }


        [HttpPost]
        public ActionResult CambiarClave(string idusuario, string claveactual, string nuevaclave, string confirmarclave)
        {
            Usuario oUsuario = new Usuario();
            oUsuario = new CN_Usuarios().Listar().Where(u => u.idUsuario == int.Parse(idusuario)).FirstOrDefault();

            if(oUsuario.Clave != CN_Recursos.ConvertirSha256(claveactual))
            {

                TempData["idUsuario"] = idusuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if(nuevaclave != confirmarclave)
            {
                TempData["idUsuario"] = idusuario;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las Nuevas Contraseñas no Coinciden";
                return View();
            }
            ViewData["vclave"] = "";

            nuevaclave = CN_Recursos.ConvertirSha256(nuevaclave);

            string mensaje = string.Empty;

            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idusuario), nuevaclave, out mensaje);

            if (respuesta)
            {
                TempData["CambioClaveExitoso"] = true;
                return RedirectToAction("Index");
            }
            else {
                TempData["idUsuario"] = idusuario;
                ViewBag.Error = mensaje;
                return View();
            }
        }
    }
}
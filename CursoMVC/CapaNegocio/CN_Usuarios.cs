using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        //Acceso de mi Capa dtos CD_Usuarios
        private CD_Usuarios objCapaDato = new CD_Usuarios();


        //creo un metodo para retorna una lista que esta en CD_Usuarios
        public List<Usuario> Listar()
        {
            return objCapaDato.Listar();
        }

        //Metodo Registrar un Nuevo Usuario
        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El Nombre del Usuario no puede ser Vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "Los Apellidos del Usuario no puede ser Vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Correo del Usuario no puede ser Vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave =CN_Recursos.GenerarClave() ;

                string asunto = "Creacion de Cuenta";
                string Mensaje_correo = "<h3>Su Cuenta fue Creada Correctamente</3></br><p>Su Contraseña para Acceder al Sistema es: !clave!</p>";
                Mensaje_correo = Mensaje_correo.Replace("!clave!", clave);

                bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo, asunto, Mensaje_correo);
                if (respuesta)
                {
                    obj.Clave = CN_Recursos.ConvertirSha256(clave);
                    return objCapaDato.Registrar(obj, out Mensaje);

                }else
                {
                    Mensaje = "No se puede enviar el correo";
                }


                return objCapaDato.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;
            }


        }


        //Metodo Editar Usuario
        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El Nombre del Usuario no puede ser Vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "Los Apellidos del Usuario no puede ser Vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Correo del Usuario no puede ser Vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }


        ///Metodo Eliminar
        public bool Eliminar(int id, out string Mensaje) {

            return objCapaDato.Eliminar(id, out Mensaje);
        }

        public bool CambiarClave(int idUsuario, string nuevaclave, out string Mensaje)
        {

            return objCapaDato.CambiarClave(idUsuario, nuevaclave, out Mensaje);
        }

        public bool Reestablecer(int idUsuario, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDato.Reestablecer(idUsuario, CN_Recursos.ConvertirSha256(nuevaclave), out Mensaje);

            if (resultado)

            {
                string asunto = "Creacion de Cuenta";
                string Mensaje_correo = "<h3>Su Cuenta Fue Reestablecida Correctamente </3></br><p>Su Contraseña para Acceder al Sistema es: !clave!</p>";
                Mensaje_correo = Mensaje_correo.Replace("!clave!", nuevaclave);

                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, Mensaje_correo);
                if (respuesta)
                {
                    return true;

                }
                else
                {
                    Mensaje = "No se puede enviar el correo";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }


        }



    }
}


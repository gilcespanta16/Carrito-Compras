using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;
using CapaNegocio;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        //Acceso de mi Capa dtos CD_Categoria
        public CD_Categoria objCapaDato = new CD_Categoria();

        //creo un metodo para retorna una lista que esta en CD_Categoria
        public List<Categoria> Listar()
        {
            return objCapaDato.Listar();
        }

        //Metodo Registrar un Nuevo Usuario
        public int Registrar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.descripcion) || string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "El Nombre de la Categoria no puede ser Vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Registrar(obj, out Mensaje);

            }
            else
            {
                return 0;
            }


        }
        


        //Metodo Editar Usuario
        public bool Editar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.descripcion) || string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "El Nombre de la Categoria no puede ser Vacio";
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
        public bool Eliminar(int id, out string Mensaje)
        {

            return objCapaDato.Eliminar(id, out Mensaje);
        }
    }
}

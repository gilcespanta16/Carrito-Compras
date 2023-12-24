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
   public class CN_Marca
    {

        //Acceso de mi Capa dtos CD_Marca
        public CD_Marca objCapaDato = new CD_Marca();

        //creo un metodo para retorna una lista que esta en CD_Marca
        public List<Marca> Listar()
        {
            return objCapaDato.Listar();
        }

        //Metodo Registrar Nuevo Marca
        public int Registrar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "El Nombre de la Marca no puede ser Vacio";
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



        //Metodo Editar Marca
        public bool Editar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "El Nombre de la Marca no puede ser Vacio";
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


        ///Metodo Eliminar Marca
        public bool Eliminar(int id, out string Mensaje)
        {

            return objCapaDato.Eliminar(id, out Mensaje);
        }
    }
}

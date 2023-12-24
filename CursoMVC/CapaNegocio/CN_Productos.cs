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
    public class CN_Productos
    {
        public CD_Productos objCapaDato = new CD_Productos();

        //creo un metodo para retorna una lista que esta en CD_Categoria
        public List<Producto> Listar()
        {
            return objCapaDato.Listar();
        }



        //Metodo Registrar Nuevo Producto
        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El Nombre del Producto no puede ser Vacio";
            }

            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripcion del Producto no puede ser Vacio";
            }

            else if (obj.oMarca.idMarca == 0)
            {
                Mensaje = "Debe seleccionar una Marca";
            }

            else if (obj.oCategoria.idCategoria == 0)
            {
                Mensaje = "Debe seleccionar una Categoria";
            }

            else if (obj.Precio == 0)
            {
                Mensaje = "Debe Ingresar el Precio del Producto";
            }

            else if (obj.Stock == 0)
            {
                Mensaje = "Debe Ingresar el Stock del Producto";
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



        //Metodo Editar Producto
        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El Nombre del Producto no puede ser Vacio";
            }

            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripcion del Producto no puede ser Vacio";
            }

            else if (obj.oMarca.idMarca == 0)
            {
                Mensaje = "Debe seleccionar una Marca";
            }

            else if (obj.oCategoria.idCategoria == 0)
            {
                Mensaje = "Debe seleccionar una Categoria";
            }

            else if (obj.Precio == 0)
            {
                Mensaje = "Debe Ingresar el Precio del Producto";
            }

            else if (obj.Stock == 0)
            {
                Mensaje = "Debe Ingresar el Stock del Producto";
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


        ///Metodo Eliminar Producto
        public bool Eliminar(int id, out string Mensaje)
        {

            return objCapaDato.Eliminar(id, out Mensaje);
        }



        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {
            return objCapaDato.GuardarDatosImagen(obj, out Mensaje);
        }


    }
}

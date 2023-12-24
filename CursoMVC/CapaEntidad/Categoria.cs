using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Security.Claims;

namespace CapaEntidad
{

    public class Categoria
    {

        public int idCategoria { get; set; }
        public string descripcion { get; set; }
        public bool Activo { get; set; }
    }
}

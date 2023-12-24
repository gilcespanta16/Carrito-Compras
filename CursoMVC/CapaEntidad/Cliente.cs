using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Cliente
    {
      public int idCliente { get; set; }
        public string Nombre { get; set; }

        public string Apeliido { get; set; }

        public string Correo { get; set; }

        public string Clave { get; set; }
        public bool Reestablecer { get; set; }
    }
}
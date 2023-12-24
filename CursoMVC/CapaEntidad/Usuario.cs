using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Security.Claims;

namespace CapaEntidad
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public bool Reestablecer { get; set; }
        public bool Activo { get; set; }

        public object FindById(int idUsuario)
        {
            throw new NotImplementedException();
        }
    }
}






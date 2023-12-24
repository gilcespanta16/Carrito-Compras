using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad

     
{
    public class Venta
    {
        public int idVenta { get; set; }
        public int idCliente { get; set; }
        public int TotalProducto { get; set; }

        public int MontoTotal { get; set; }
        public string Contacto { get; set; }

        public string idDistrito { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        public string FechaTexto { get; set; }

        public string idTransaccion { get; set; }  //Numero o Token  Para Paypal poder identifica un pago

        public List<DetalleVenta> oDetalleVenta { get; set; } //hace referencia a mi tabla detalle venta
    }
}

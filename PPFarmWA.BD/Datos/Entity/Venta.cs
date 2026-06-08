using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.BD.Datos.Entity
{
    public class Venta : EntityBase
    {
        public int idJugadorVendedor { get; set; }
        public int idJugadorComprador { get; set; }
        public int cantidad_venta { get; set; }
        public double precio_venta { get; set; }
    }
}

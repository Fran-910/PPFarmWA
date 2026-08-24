using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Shared.DTO
{
    public class VentaDTO
    {
        public int Id { get; set; }

        public int idJugadorVendedor { get; set; }

        public int idJugadorComprador { get; set; }

        public int cantidadVenta { get; set; }

        public double precioVenta { get; set; }
    }
}

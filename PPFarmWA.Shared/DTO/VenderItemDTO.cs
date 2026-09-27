using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Shared.DTO
{
    public class VenderItemDTO
    {
        public int idJugador { get; set; }
        public int idItem { get; set; }
        public int cantidad { get; set; }
        public double precioVenta { get; set; }
    }
}

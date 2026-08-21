using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PPFarmWA.BD.Datos.Entity
{
    public class Venta : EntityBase
    {
        [Required]
        public int idJugadorVendedor { get; set; }
        [Required]
        public int idJugadorComprador { get; set; }
        [Required]
        public int cantidadVenta { get; set; }
        [Required]
        public double precioVenta { get; set; } = 0;
    }
}

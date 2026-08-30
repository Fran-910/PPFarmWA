using PPFarmWA.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PPFarmWA.BD.Datos.Entity
{
    public class Item : EntityBase
    {
        public int cantidad { get; set; } = 1;
        public int JugadorId { get; set; }
        public Jugador Jugador { get; set; }
        public int RecursoId { get; set; }
        public Recurso Recurso {  get; set; }
        public int? VentaId { get; set; }
        public Venta? Venta { get; set; }
    }
}

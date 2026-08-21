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
        public int idJugador { get; set; }
        public int idRecurso { get; set; }
        public int idVenta { get; set; }
    }
}

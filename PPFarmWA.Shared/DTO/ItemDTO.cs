using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Shared.DTO
{
    public class ItemDTO
    {
        public int Id { get; set; }

        public int cantidad { get; set; }

        public int idJugador { get; set; }

        public int idRecurso { get; set; }

        public int? idVenta { get; set; }
    }
}

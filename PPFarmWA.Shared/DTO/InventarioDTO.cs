using PPFarmWA.Shared.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Shared.DTO
{
    public class InventarioDTO
    {
        public int Id { get; set; }

        public int cantidad { get; set; }

        public int idJugador { get; set; }

        public int idRecurso { get; set; }

        public string nombre { get; set; }

        public string descripcion { get; set; }

        public int eficiencia { get; set; }

        public int durabilidad { get; set; }

        public double valor { get; set; }

        public TipoItemEnum tipo { get; set; }

        public RarezaEnum idRareza { get; set; }
    }
}

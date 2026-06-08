using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.BD.Datos.Entity
{
    public class Recurso : EntityBase
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public int durabilidad { get; set; } = 10;
        public double valor { get; set; } = 0;
        public int idTipo { get; set; }
        public int idRareza { get; set; }
    }
}

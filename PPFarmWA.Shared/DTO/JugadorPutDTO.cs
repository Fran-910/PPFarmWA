using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Shared.DTO
{
    public class JugadorPutDTO
    {
        public int Id { get; set; }
        public double ppCoins { get; set; }
        public int points { get; set; }
        public int level { get; set; }
        public int experiencia { get; set; }
        public int idUltimaHerramienta { get; set; }
    }
}

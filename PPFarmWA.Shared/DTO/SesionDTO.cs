using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Shared.DTO
{
    public class SesionDTO
    {
        public int JugadorId { get; set; }
        public string userName { get; set; }
        public string email { get; set; }

        public double ppCoins { get; set; }

        public int points { get; set; }

        public int level { get; set; }

        public int experiencia { get; set; }

        public int IdUltimaHerramienta { get; set; }
        public List<ItemDTO> Items { get; set; }

    }
}

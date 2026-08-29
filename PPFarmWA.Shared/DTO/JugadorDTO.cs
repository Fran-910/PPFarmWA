using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Shared.DTO
{
    public class JugadorDTO
    {
        public int Id { get; set; }

        public string userName { get; set; }
        public string password { get; set; }

        public string email { get; set; }

        public double ppCoins { get; set; }

        public int points { get; set; }

        public int level { get; set; }

        public int experiencia { get; set; }

        public bool esTienda { get; set; }

        public bool esAdmin { get; set; }

        public int idUltimaHerramienta { get; set; }

        public int idUltimoDispositivo { get; set; }

        public int idUltimoPotenciador { get; set; }
    }
}

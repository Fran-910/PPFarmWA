using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.BD.Datos.Entity
{
    public class Jugador : EntityBase
    {
        public string user_name { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public double pp_coins { get; set; } = 100;
        public int points { get; set; } = 0;    
        public int level { get; set; } = 1;
        public int experiencia { get; set; } = 0;
        public bool es_tienda { get; set; } = false;
        public bool es_admin { get; set; } = false;

    }
}

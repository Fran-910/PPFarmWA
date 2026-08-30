using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PPFarmWA.BD.Datos.Entity
{
    public class Jugador : EntityBase
    {
        [Required]
        [MinLength(3)]
        [StringLength(20)]
        public string userName { get; set; }
        [Required]
        [MinLength(5)]
        [StringLength(100)]
        public string password { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        public double ppCoins { get; set; } = 100;
        public int points { get; set; } = 0;    
        public int level { get; set; } = 1;
        public int experiencia { get; set; } = 0;
        public bool esTienda { get; set; } = false;
        public bool esAdmin { get; set; } = false;
        public int idUltimaHerramienta { get; set; }
        public int idUltimoDispositivo { get; set; }
        public int idUltimoPotenciador { get; set; }
    }
}

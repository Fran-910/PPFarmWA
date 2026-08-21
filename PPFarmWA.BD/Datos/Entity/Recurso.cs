using PPFarmWA.Shared.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PPFarmWA.BD.Datos.Entity
{
    public class Recurso : EntityBase
    {
        [Required]
        [MaxLength(50)]
        public string nombre { get; set; }
        [MaxLength(150)]
        public string descripcion { get; set; }
        public int eficiencia { get; set; } = 1;
        public int durabilidad { get; set; } = 10;
        public double valor { get; set; } = 0;
        public bool deTienda { get; set; } = false;
        public TipoItemEnum tipo { get; set; } = TipoItemEnum.Herramienta;
        public RarezaEnum idRareza { get; set; } = RarezaEnum.Comun;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Shared.DTO
{
    public class SesionDTO
    {
        public int JugadorId { get; set; }
        public string Nombre { get; set; }
        public int Nivel { get; set; }
        public int Exp { get; set; }
        public int Puntos { get; set; }
        public int Ppcoins { get; set; }
        public int IdUltimaHerramienta { get; set; }
        public List<int> Items { get; set; }

    }
}

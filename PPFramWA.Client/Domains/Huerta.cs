using PPFarmWA.Shared.Catalogos;
using System.Numerics;
namespace PPFramWA.Client.Domains
{
    public class Huerta
    {
        public Jugador __jugador;

        public Huerta(Jugador jugador)
        {
            __jugador = jugador;
        }

        public int cantCultivos()
        {
            var cantCeldas = 0;

            var umbralDeNivelActual = CeldasHuerta.values.Where(x => x.Key > __jugador.level).MinBy(x => x.Value).Key;

            cantCeldas = CeldasHuerta.values[umbralDeNivelActual];

            return cantCeldas ;
        }

    }
}

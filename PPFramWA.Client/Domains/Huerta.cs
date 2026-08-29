using PPFarmWA.Shared.Catalogos;
using PPFramWA.Client.Services;
using System.Numerics;
namespace PPFramWA.Client.Domains
{
    public class Huerta
    {
        public int cantCeldas { get; set; }
        public JugadorState jugadorStateHuerta;

        public Huerta(JugadorState jugadorState)
        {
            jugadorStateHuerta = jugadorState;
            jugadorStateHuerta.__jugador!.OnSubioNivel += calcularCultivos;
            calcularCultivos();
        }
        
        public void calcularCultivos()
        {
            var umbralDeNivelActual = CeldasHuerta.values.Where(x => x.Key > jugadorStateHuerta.__jugador!.level).MinBy(x => x.Value).Key;

            cantCeldas = CeldasHuerta.values[umbralDeNivelActual];
        }
    }
}

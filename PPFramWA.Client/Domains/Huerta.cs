using PPFarmWA.Shared.Catalogos;
using PPFramWA.Client.Services;
using System.Numerics;
namespace PPFramWA.Client.Domains
{
    public class Huerta : IDisposable
    {
        public int cantCeldas { get; set; }
        public int columnas { get; set; }
        public JugadorState jugadorStateHuerta;
        private readonly Jugador jugador;

        public Huerta(JugadorState jugadorState)
        {
            jugadorStateHuerta = jugadorState;
            jugador = jugadorStateHuerta.__jugador!;
            jugador.OnSubioNivel += calcularCultivos;
            calcularCultivos();
        }

        public void Dispose()
        {
            jugador.OnSubioNivel -= calcularCultivos;
        }

        public void calcularCultivos()
        {
            var umbralDeNivelActual = CeldasHuerta.values.Where(x => x.Key > jugadorStateHuerta.__jugador!.level).MinBy(x => x.Value).Key;

            cantCeldas = CeldasHuerta.values[umbralDeNivelActual];
            columnas = CeldasHuerta.ColumnasPara(cantCeldas);
        }
    }
}

using PPFramWA.Client.Domains;

namespace PPFramWA.Client.Services
{
    public class JugadorState
    {
        public Jugador? __jugador { get; set; }

        public void EstablecerJugador(Jugador? jugador)
        {
            __jugador = jugador;
        }
    }
}

using PPFramWA.Client.Domains;

namespace PPFramWA.Client.Services
{
    public class JugadorState
    {
        public Jugador? __jugador { get; set; }
        public event Action? OnChange;
        public void EstablecerJugador(Jugador? jugador)
        {
            __jugador = jugador;
        }
        public void NotificarCambios()
        {
            OnChange?.Invoke();
        }

        // Guardado del estado del jugador

        // Método de guardado automático del estado del jugador
    }
}

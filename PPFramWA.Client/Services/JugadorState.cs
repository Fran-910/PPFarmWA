using PPFarmWA.Shared.DTO;
using PPFramWA.Client.Domains;

namespace PPFramWA.Client.Services
{
    public class JugadorState
    {
        public Jugador? __jugador { get; set; }
        public event Action? OnChange;
        public void EstablecerJugador(SesionDTO dto)
        {
            Jugador jugador = new Jugador(dto);   
            __jugador = jugador;
            NotificarCambios();
        }
        public void NotificarCambios()
        {
            OnChange?.Invoke();
        }

        // Guardado del estado del jugador

        // Método de guardado automático del estado del jugador
    }
}

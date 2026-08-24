using PPFarmWA.Shared.DTO;

namespace PPFramWA.Client.Services
{
    public class VentaServicio
    {
        private readonly ApiServicio _api;

        public VentaServicio(ApiServicio api)
        {
            _api = api;
        }

        public async Task<List<VentaDTO>> ObtenerTodos()
        {
            return await _api.GetAsync<List<VentaDTO>>(
                "api/Venta") ?? new List<VentaDTO>();
        }

        public async Task<List<VentaDTO>> ObtenerVentasJugador(int idJugador)
        {
            return await _api.GetAsync<List<VentaDTO>>(
                $"api/Venta/jugador/{idJugador}")
                ?? new List<VentaDTO>();
        }

        public async Task<VentaDTO?> ObtenerPorId(int id)
        {
            return await _api.GetAsync<VentaDTO>(
                $"api/Venta/{id}");
        }
    }
}

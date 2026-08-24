using PPFarmWA.Shared.DTO;

namespace PPFramWA.Client.Services
{
    public class JugadorServicio
    {
        private readonly ApiServicio _api;

        public JugadorServicio(ApiServicio api)
        {
            _api = api;
        }

        public async Task<JugadorDTO?> ObtenerPorId(int id)
        {
            return await _api.GetAsync<JugadorDTO>(
                $"api/Jugador/{id}");
        }

        public async Task<List<JugadorDTO>> ObtenerTodos()
        {
            return await _api.GetAsync<List<JugadorDTO>>(
                "api/Jugador") ?? new List<JugadorDTO>();
        }
    }
}

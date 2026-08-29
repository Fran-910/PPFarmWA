using PPFarmWA.Shared.DTO;

namespace PPFramWA.Client.Services
{
    public class ItemServicio
    {
        private readonly ApiServicio _api;

        public ItemServicio(ApiServicio api)
        {
            _api = api;
        }

        public async Task<List<ItemDTO>> ObtenerTodos()
        {
            return await _api.GetAsync<List<ItemDTO>>(
                "api/Item") ?? new List<ItemDTO>();
        }

        public async Task<List<InventarioDTO>> ObtenerInventario(int idJugador)
        {
            return await _api.GetAsync<List<InventarioDTO>>(
                $"api/Item/jugador/{idJugador}")
                ?? new List<InventarioDTO>();
        }

        public async Task<ItemDTO?> ObtenerPorId(int id)
        {
            return await _api.GetAsync<ItemDTO>(
                $"api/Item/{id}");
        }
    }
}

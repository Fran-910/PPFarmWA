using PPFarmWA.Shared.DTO;

namespace PPFramWA.Client.Services
{
    public class RecursoServicio
    {
        private readonly ApiServicio _api;

        public RecursoServicio(ApiServicio api)
        {
            _api = api;
        }

        public async Task<List<RecursoDTO>> ObtenerTodos()
        {
            return await _api.GetAsync<List<RecursoDTO>>(
                "api/Recurso") ?? new List<RecursoDTO>();
        }

        public async Task<List<RecursoDTO>> ObtenerRecursosTienda()
        {
            return await _api.GetAsync<List<RecursoDTO>>(
                "api/Recurso/tienda") ?? new List<RecursoDTO>();
        }

        public async Task<RecursoDTO?> ObtenerPorId(int id)
        {
            return await _api.GetAsync<RecursoDTO>(
                $"api/Recurso/{id}");
        }
    }
}

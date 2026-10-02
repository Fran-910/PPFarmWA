using PPFramWA.Client.Domains;
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

        public async Task<HttpResponseMessage> GuardarPartida(JugadorDTO jugador)
        {
            return await _api.PutAsync<JugadorDTO>(
                $"api/Jugador/{jugador.Id}", jugador);
        }

        public async Task<(bool Exito, string Mensaje)> GuardarPartida(Jugador jugador)
        {
            var dto = new JugadorDTO
            {
                Id = jugador.Id,
                userName = jugador.userName,
                level = jugador.level,
                experiencia = jugador.experiencia,
                ppCoins = jugador.ppCoins,
                points = jugador.points,
                idUltimaHerramienta = jugador.idUltimaHerramienta
            };

            var respuesta = await _api.PutAsync<JugadorDTO>(
                $"api/Jugador/{dto.Id}", dto);

            if (!respuesta.IsSuccessStatusCode)
            {
                var detalle = await respuesta.Content.ReadAsStringAsync();
                return (false, string.IsNullOrWhiteSpace(detalle)
                    ? "No se pudo guardar la partida."
                    : detalle.Trim('"'));
            }

            return (true, "Partida guardada.");
        }

    }
}

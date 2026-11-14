
using PPFramWA.Client.Domains;
using PPFarmWA.Shared.DTO;
using System.Text.Json;

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

        // Obtener la configuración económica desde el servidor.
        public async Task<EconomiaJuegoDTO?> ObtenerConfiguracionEconomica()
        {
            return await _api.GetAsync<EconomiaJuegoDTO>(
                "api/Jugador/economia");
        }

        public async Task<(bool Exito, string Mensaje)> GuardarPartida(
            Jugador jugador)
        {
            var dto = new JugadorPutDTO
            {
                Id = jugador.Id,
                level = jugador.level,
                experiencia = jugador.experiencia,
                ppCoins = jugador.ppCoins,
                points = jugador.points,
                idUltimaHerramienta = jugador.idUltimaHerramienta,
                items = jugador.items
            };

            var respuesta = await _api.PutAsync<JugadorPutDTO>(
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

        // Solicitar al servidor la recompensa por cosechar.
        // El cliente no decide la cantidad de Points.
        public async Task<(bool Exito, string Mensaje, int Points, int Recompensa)>
            AcreditarPointsPorCosecha(int idJugador)
        {
            if (idJugador <= 0)
            {
                return (
                    false,
                    "El ID del jugador no es válido.",
                    0,
                    0
                );
            }

            var respuesta = await _api.PostAsync(
                $"api/Jugador/{idJugador}/recompensa-points",
                new { });

            if (!respuesta.IsSuccessStatusCode)
            {
                var detalle = await respuesta.Content.ReadAsStringAsync();

                return (
                    false,
                    string.IsNullOrWhiteSpace(detalle)
                        ? "No se pudo acreditar la recompensa."
                        : detalle.Trim('"'),
                    0,
                    0
                );
            }

            var contenido = await respuesta.Content.ReadAsStringAsync();

            try
            {
                var resultado =
                    JsonSerializer.Deserialize<RecompensaPointsRespuesta>(
                        contenido,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (resultado == null)
                {
                    return (
                        false,
                        "El servidor no devolvió los datos de la recompensa.",
                        0,
                        0
                    );
                }

                return (
                    true,
                    resultado.Mensaje ??
                        "Points acreditados correctamente.",
                    resultado.Points,
                    resultado.Recompensa
                );
            }
            catch (JsonException)
            {
                return (
                    false,
                    "No se pudo interpretar la respuesta del servidor.",
                    0,
                    0
                );
            }
        }

        private class RecompensaPointsRespuesta
        {
            public string? Mensaje { get; set; }

            public int Points { get; set; }

            public int Recompensa { get; set; }
        }
    }

    // Datos de configuración económica recibidos del servidor.
    public class EconomiaJuegoDTO
    {
        public decimal TasaConversion { get; set; }

        public double LimiteGlobalPPCoins { get; set; }

        public int PointsPorNivelCosecha { get; set; }
    }
}

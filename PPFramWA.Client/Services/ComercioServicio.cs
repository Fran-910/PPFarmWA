
using PPFarmWA.Shared.DTO;
using System.Net.Http;
using System.Text.Json;

namespace PPFramWA.Client.Services
{
    public class ComercioServicio
    {
        private readonly ApiServicio _api;

        public ComercioServicio(ApiServicio api)
        {
            _api = api;
        }

        public async Task<(bool Exito, string Mensaje)> Comprar(
            CompraDTO compra)
        {
            var respuesta = await _api.PostAsync(
                "api/Compra",
                compra);

            return (
                respuesta.IsSuccessStatusCode,
                await LeerMensaje(respuesta)
            );
        }

        public async Task<(bool Exito, string Mensaje)> Vender(
            VenderItemDTO venta)
        {
            var respuesta = await _api.PostAsync(
                "api/Item/vender",
                venta);

            return (
                respuesta.IsSuccessStatusCode,
                await LeerMensaje(respuesta)
            );
        }

        // Conversión de Points a PP Coins.
        // Utiliza POST api/Conversor/convertir.
        public async Task<(bool Exito, string Mensaje)> ConvertirPoints(
            int idJugador,
            int puntosAConvertir)
        {
            if (idJugador <= 0 || puntosAConvertir <= 0)
            {
                return (
                    false,
                    "Los datos de conversión no son válidos."
                );
            }

            var request = new
            {
                IdJugador = idJugador,
                PuntosAConvertir = puntosAConvertir
            };

            var respuesta = await _api.PostAsync(
                "api/Conversor/convertir",
                request);

            return (
                respuesta.IsSuccessStatusCode,
                await LeerMensaje(respuesta)
            );
        }

        // Lee correctamente el mensaje devuelto por el servidor.
        private static async Task<string> LeerMensaje(
            HttpResponseMessage respuesta)
        {
            var texto = await respuesta.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(texto))
            {
                return respuesta.IsSuccessStatusCode
                    ? "Operación realizada correctamente."
                    : "No se pudo completar la operación.";
            }

            try
            {
                using var documento =
                    JsonDocument.Parse(texto);

                if (documento.RootElement.ValueKind ==
                    JsonValueKind.Object)
                {
                    foreach (var propiedad in
                        documento.RootElement.EnumerateObject())
                    {
                        if (string.Equals(
                            propiedad.Name,
                            "mensaje",
                            StringComparison.OrdinalIgnoreCase))
                        {
                            return propiedad.Value.GetString()
                                ?? "Operación realizada correctamente.";
                        }
                    }
                }
            }
            catch (JsonException)
            {
                // La respuesta también puede ser texto simple.
            }

            return texto.Trim('"');
        }
    }
}

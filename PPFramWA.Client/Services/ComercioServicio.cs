using PPFarmWA.Shared.DTO;
using PPFramWA.Client.Services;
using System.Net.Http;

namespace PPFarmWA.Client.Services
{
    public class ComercioServicio
    {
        private readonly ApiServicio _api;

        public ComercioServicio(ApiServicio api)
        {
            _api = api;
        }

        public async Task<(bool Exito, string Mensaje)> Comprar(CompraDTO compra)
        {
            var respuesta = await _api.PostAsync("api/Compra", compra);

            return (
                respuesta.IsSuccessStatusCode,
                await LeerMensaje(respuesta)
            );
        }

        public async Task<(bool Exito, string Mensaje)> Vender(VenderItemDTO venta)
        {
            var respuesta = await _api.PostAsync("api/Item/vender", venta);

            return (
                respuesta.IsSuccessStatusCode,
                await LeerMensaje(respuesta)
            );
        }

        public async Task<(bool Exito, string Mensaje)> ConvertirPoints(int idJugador, int puntosAConvertir)
        {
            var request = new
            {
                IdJugador = idJugador,
                PuntosAConvertir = puntosAConvertir
            };
            var respuesta = await _api.PostAsync("api/Conversor/convertir", request);
            return (respuesta.IsSuccessStatusCode, await LeerMensaje(respuesta));
        }

    private static async Task<string> LeerMensaje(HttpResponseMessage respuesta)
        {
            var texto = await respuesta.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(texto)
                ? (respuesta.IsSuccessStatusCode ? "Operación realizada correctamente." : "No se pudo completar la operación.")
                : texto.Trim('"');
        }
    }
}

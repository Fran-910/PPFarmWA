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

        public async Task <(bool Exito, string Mensaje)> convertirpoints(int puntosconvertidos)
        {
            await Task.Delay(1000);
            return (true, $"Se han convertido {puntosconvertidos} puntos a PP Coins.");
        }
        private static async Task<string> LeerMensaje(HttpResponseMessage respuesta)
        {
            var texto = await respuesta.Content.ReadAsStringAsync();

            return string.IsNullOrWhiteSpace(texto)
                ? (respuesta.IsSuccessStatusCode
                    ? "Operación realizada correctamente."
                    : "No se pudo completar la operación.")
                : texto.Trim('"');
        }

        public async Task<RespuestaFalsa> ConvertirPoints(int puntosConvertidos)
        {
            await Task.Delay(1000);

            return new RespuestaFalsa
            {
                Exito = true,
                Mensaje = $"¡Conversión exitosa! El servidor aprobó tus {puntosConvertidos} Points."
            };
        }
    }

    public class RespuestaFalsa
    {
        public bool Exito { get; set; }
        public string? Mensaje { get; set; }
    }
}
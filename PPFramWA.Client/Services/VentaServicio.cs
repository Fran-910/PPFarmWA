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


        public async Task<List<VentaDTO>> ObtenerDisponibles()
        {
            return await _api.GetAsync<List<VentaDTO>>(
                "api/Venta/disponibles") ?? new List<VentaDTO>();
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


        public async Task<bool> CrearVenta(VentaDTO venta)
        {
            var respuesta = await _api.PostAsync(
                "api/Venta",
                venta);

            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> PublicarIntercambio(int itemId, VentaDTO venta)
        {
            var respuesta = await _api.PostAsync(
                $"api/Venta/publicar/{itemId}",
                venta);

            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> ComprarIntercambio(int idVenta, int idJugadorComprador)
        {
            var respuesta = await _api.PostAsync(
                $"api/Venta/{idVenta}/comprar?idJugadorComprador={idJugadorComprador}",
                new { });

            return respuesta.IsSuccessStatusCode;
        }
    }
}

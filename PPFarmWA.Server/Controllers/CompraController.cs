using Microsoft.AspNetCore.Mvc;
using PPFarmWA.BD.Datos.Entity;
using PPFarmWA.Repositorio.Repositorios;
using PPFarmWA.Shared.DTO;

namespace PPFarmWA.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompraController : ControllerBase
    {
        private readonly IJugadorRepositorio _jugadorRepositorio;
        private readonly IRecursoRepositorio _recursoRepositorio;
        private readonly IItemRepositorio _itemRepositorio;

        public CompraController(
            IJugadorRepositorio jugadorRepositorio,
            IRecursoRepositorio recursoRepositorio,
            IItemRepositorio itemRepositorio)
        {
            _jugadorRepositorio = jugadorRepositorio;
            _recursoRepositorio = recursoRepositorio;
            _itemRepositorio = itemRepositorio;
        }

        [HttpPost]
        public async Task<IActionResult> Comprar(CompraDTO dto)
        {
            // 1. Validar cantidad
            if (dto.cantidad < 1 || dto.cantidad > 3)
                return BadRequest("La cantidad debe estar entre 1 y 3.");

            // 2. Buscar jugador
            var jugador = await _jugadorRepositorio.GetByIdAsync(dto.idJugador);

            if (jugador == null)
                return NotFound("El jugador no existe.");

            // 3. Buscar recurso
            var recurso = await _recursoRepositorio.GetByIdAsync(dto.idRecurso);

            if (recurso == null)
                return NotFound("El recurso no existe.");

            // 4. Comprobar que esté disponible en tienda
            if (!recurso.deTienda)
                return BadRequest("Este recurso no está disponible en la tienda.");

            // 5. Calcular precio total
            double precioTotal = recurso.valor * dto.cantidad;

            // 6. Comprobar PP Coins
            if (jugador.ppCoins < precioTotal)
                return BadRequest("El jugador no tiene suficientes PP Coins.");

            // 7. Buscar si ya tiene ese recurso en el inventario
            var inventario = await _itemRepositorio
                .GetInventarioJugadorAsync(dto.idJugador);

            var itemExistente = inventario
                .FirstOrDefault(i => i.idRecurso == dto.idRecurso);

            // 8. Descontar PP Coins
            var coinsModificadas = await _jugadorRepositorio
                .ModificarCoinsAsync(dto.idJugador, -precioTotal);

            if (!coinsModificadas)
                return BadRequest("No se pudieron modificar las PP Coins.");

            // 9. Si ya tiene el recurso, aumentar cantidad
            if (itemExistente != null)
            {
                itemExistente.cantidad += dto.cantidad;

                await _itemRepositorio.UpdateAsync(itemExistente);
            }
            else
            {
                // Si no lo tiene, crear nuevo Item
                var nuevoItem = new Item
                {
                    cantidad = dto.cantidad,
                    idJugador = dto.idJugador,
                    idRecurso = dto.idRecurso,
                    idVenta = 0
                };

                await _itemRepositorio.AddAsync(nuevoItem);
            }

            return Ok(new
            {
                mensaje = "Compra realizada correctamente.",
                recurso = recurso.nombre,
                cantidad = dto.cantidad,
                precioTotal = precioTotal
            });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPFarmWA.BD.Datos;
using PPFarmWA.BD.Datos.Entity;
using PPFarmWA.Repositorio.Repositorios;
using PPFarmWA.Shared.DTO;

namespace PPFarmWA.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompraController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CompraController(
            AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Comprar(CompraDTO dto)
        {
            // 1. Validar cantidad
            if (dto.cantidad < 1)
                return BadRequest("La cantidad debe ser mayor a cero.");

            // 2. Buscar jugador
            var jugador = await _context.Jugadores.FindAsync(dto.idJugador);

            if (jugador == null)
                return NotFound("El jugador no existe.");

            // 3. Buscar recurso
            var recurso = await _context.Recursos.FindAsync(dto.idRecurso);

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
            await using var transaccion = await _context.Database.BeginTransactionAsync();
            try
            {
                var itemExistente = await _context.Items.FirstOrDefaultAsync(i =>
                    i.JugadorId == dto.idJugador && i.RecursoId == dto.idRecurso);

                jugador.ppCoins -= precioTotal;
                if (itemExistente is null)
                {
                    _context.Items.Add(new Item
                    {
                        cantidad = dto.cantidad,
                        JugadorId = dto.idJugador,
                        RecursoId = dto.idRecurso
                    });
                }
                else
                {
                    itemExistente.cantidad += dto.cantidad;
                }

                await _context.SaveChangesAsync();
                await transaccion.CommitAsync();
            }
            catch
            {
                await transaccion.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo completar la compra.");
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

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPFarmWA.BD.Datos;
using PPFarmWA.BD.Datos.Entity;
using PPFarmWA.Repositorio.Repositorios;
using PPFarmWA.Shared.DTO;
using PPFarmWA.Shared.Enum;

namespace PPFarmWA.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepositorio _repositorio;
        private readonly AppDbContext _context;

        public ItemController(IItemRepositorio repositorio, AppDbContext context)
        {
            _repositorio = repositorio;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> Get()
        {
            var items = await _repositorio.GetAllAsync();

            var resultado = items.Select(i => new ItemDTO
            {
                Id = i.Id,
                cantidad = i.cantidad,
                durabilidad = i.durabilidad,
                idJugador = i.JugadorId,
                idRecurso = i.RecursoId,
                idVenta = i.VentaId
            });

            return Ok(resultado);
        }

        [HttpGet("jugador/{idJugador}")]
        public async Task<ActionResult<IEnumerable<InventarioDTO>>> GetInventario(int idJugador)
        {
            var items = await _repositorio.GetInventarioJugadorAsync(idJugador);

            var resultado = items
                .Join(
                    _context.Recursos,
                    item => item.RecursoId,
                    recurso => recurso.Id,
                    (item, recurso) => new InventarioDTO
                    {
                        Id = item.Id,
                        cantidad = item.cantidad,
                        idJugador = item.JugadorId,
                        idRecurso = item.RecursoId,

                        nombre = recurso.nombre,
                        descripcion = recurso.descripcion,

                        eficiencia = recurso.eficiencia,
                        durabilidad = recurso.durabilidad,
                        durabilidadItem = item.durabilidad,
                        valor = recurso.valor,

                        tipo = recurso.tipo,
                        idRareza = recurso.idRareza
                    })
                .ToList();

            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetById(int id)
        {
            var item = await _repositorio.GetByIdAsync(id);

            if (item == null)
                return NotFound();

            var dto = new ItemDTO
            {
                Id = item.Id,
                cantidad = item.cantidad,
                durabilidad = item.durabilidad,
                idJugador = item.JugadorId,
                idRecurso = item.RecursoId,
                idVenta = item.VentaId
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<ItemDTO>> Post(ItemDTO dto)
        {
            var item = new Item
            {
                cantidad = dto.cantidad,
                durabilidad = dto.durabilidad,
                JugadorId = dto.idJugador,
                RecursoId = dto.idRecurso,
                VentaId = (dto.idVenta.HasValue && dto.idVenta.Value > 0) ? dto.idVenta : null
            };

            // Si no se especifica durabilidad, tomar la del recurso base
            if (item.durabilidad == 0)
            {
                var recursoBase = await _context.Recursos.FindAsync(item.RecursoId);
                if (recursoBase != null)
                {
                    item.durabilidad = recursoBase.durabilidad;
                }
            }

            var creado = await _repositorio.AddAsync(item);

            dto.Id = creado.Id;

            return CreatedAtAction(
                nameof(GetById),
                new { id = creado.Id },
                dto
            );
        }

        [HttpPost("vender")]
        public async Task<IActionResult> Vender(VenderItemDTO dto)
        {
            if (dto.idJugador <= 0 || dto.idItem <= 0 || dto.cantidad < 1)
            {
                return BadRequest("Los datos de la venta no son válidos.");
            }

            // La transacción comienza antes de consultar el inventario
            // para proteger la operación ante ventas simultáneas.
            await using var transaccion =
                await _context.Database.BeginTransactionAsync(
                    System.Data.IsolationLevel.Serializable);

            try
            {
                var item = await _context.Items
                    .Include(i => i.Recurso)
                    .FirstOrDefaultAsync(i =>
                        i.Id == dto.idItem &&
                        i.JugadorId == dto.idJugador);

                if (item is null)
                {
                    await transaccion.RollbackAsync();
                    return NotFound("El objeto no pertenece al jugador.");
                }

                if (item.Recurso is null)
                {
                    await transaccion.RollbackAsync();
                    return BadRequest("No se encontró el recurso del objeto.");
                }

                if (item.cantidad < dto.cantidad)
                {
                    await transaccion.RollbackAsync();
                    return BadRequest("No tenés esa cantidad disponible.");
                }

                var jugador = await _context.Jugadores
                    .FirstOrDefaultAsync(j => j.Id == dto.idJugador);

                if (jugador is null)
                {
                    await transaccion.RollbackAsync();
                    return NotFound("El jugador no existe.");
                }

                // El servidor obtiene el valor real del recurso.
                // Se paga el 50 % del valor por cada unidad vendida.
                var valorUnitario = item.Recurso.valor;

                if (valorUnitario < 0)
                {
                    await transaccion.RollbackAsync();
                    return BadRequest("El valor del recurso no es válido.");
                }

                var precioVenta = Math.Round(
                    valorUnitario * dto.cantidad * 0.50,
                    2,
                    MidpointRounding.AwayFromZero);

                // Descontar las unidades vendidas.
                item.cantidad -= dto.cantidad;

                // Acreditar las PP Coins calculadas en el servidor.
                jugador.ppCoins += precioVenta;

                // Registrar la venta al sistema del juego.
                _context.Ventas.Add(new Venta
                {
                    idJugadorVendedor = dto.idJugador,
                    idJugadorComprador = 0,
                    cantidadVenta = dto.cantidad,
                    precioVenta = precioVenta
                });

                // Si no quedan unidades, eliminar el objeto del inventario.
                if (item.cantidad == 0)
                {
                    if (jugador.idUltimaHerramienta == item.Id)
                        jugador.idUltimaHerramienta = 0;

                    if (jugador.idUltimoDispositivo == item.Id)
                        jugador.idUltimoDispositivo = 0;

                    if (jugador.idUltimoPotenciador == item.Id)
                        jugador.idUltimoPotenciador = 0;

                    _context.Items.Remove(item);
                }

                await _context.SaveChangesAsync();
                await transaccion.CommitAsync();

                return Ok(
                    $"Venta realizada correctamente. Recibiste {precioVenta} PP Coins.");
            }
            catch (Exception)
            {
                await transaccion.RollbackAsync();

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "No se pudo completar la venta.");
            }
        }
    }
}
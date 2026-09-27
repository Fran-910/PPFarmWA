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
                JugadorId = dto.idJugador,
                RecursoId = dto.idRecurso,
                VentaId = (dto.idVenta.HasValue && dto.idVenta.Value > 0) ? dto.idVenta : null
            };

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
            if (dto.cantidad < 1 || dto.precioVenta < 0)
                return BadRequest("La cantidad y el precio de venta no son válidos.");

            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == dto.idItem && i.JugadorId == dto.idJugador);
            if (item is null)
                return NotFound("El item no pertenece al jugador.");
            if (item.cantidad < dto.cantidad)
                return BadRequest("No tenés esa cantidad disponible.");

            var jugador = await _context.Jugadores.FindAsync(dto.idJugador);
            if (jugador is null)
                return NotFound("El jugador no existe.");

            await using var transaccion = await _context.Database.BeginTransactionAsync();
            try
            {
                item.cantidad -= dto.cantidad;
                jugador.ppCoins += dto.precioVenta;
                _context.Ventas.Add(new Venta
                {
                    idJugadorVendedor = dto.idJugador,
                    idJugadorComprador = 0,
                    cantidadVenta = dto.cantidad,
                    precioVenta = dto.precioVenta
                });

                if (item.cantidad == 0)
                {
                    if (jugador.idUltimaHerramienta == item.Id) jugador.idUltimaHerramienta = 0;
                    if (jugador.idUltimoDispositivo == item.Id) jugador.idUltimoDispositivo = 0;
                    if (jugador.idUltimoPotenciador == item.Id) jugador.idUltimoPotenciador = 0;
                    _context.Items.Remove(item);
                }

                await _context.SaveChangesAsync();
                await transaccion.CommitAsync();
                return Ok("Venta realizada correctamente.");
            }
            catch
            {
                await transaccion.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, "No se pudo completar la venta.");
            }
        }
    }
}
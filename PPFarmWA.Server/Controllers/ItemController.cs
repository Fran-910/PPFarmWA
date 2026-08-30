using Microsoft.AspNetCore.Mvc;
using PPFarmWA.BD.Datos;
using PPFarmWA.BD.Datos.Entity;
using PPFarmWA.Repositorio.Repositorios;
using PPFarmWA.Shared.DTO;

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
    }
}
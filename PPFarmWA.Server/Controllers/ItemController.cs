using Microsoft.AspNetCore.Mvc;
using PPFarmWA.Repositorio.Repositorios;
using PPFarmWA.Shared.DTO;

namespace PPFarmWA.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepositorio _repositorio;

        public ItemController(IItemRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> Get()
        {
            var items = await _repositorio.GetAllAsync();

            var resultado = items.Select(i => new ItemDTO
            {
                Id = i.Id,
                cantidad = i.cantidad,
                idJugador = i.idJugador,
                idRecurso = i.idRecurso,
                idVenta = i.idVenta
            });

            return Ok(resultado);
        }

        [HttpGet("jugador/{idJugador}")]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetInventario(int idJugador)
        {
            var items = await _repositorio.GetInventarioJugadorAsync(idJugador);

            var resultado = items.Select(i => new ItemDTO
            {
                Id = i.Id,
                cantidad = i.cantidad,
                idJugador = i.idJugador,
                idRecurso = i.idRecurso,
                idVenta = i.idVenta
            });

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
                idJugador = item.idJugador,
                idRecurso = item.idRecurso,
                idVenta = item.idVenta
            };

            return Ok(dto);
        }
    }
}
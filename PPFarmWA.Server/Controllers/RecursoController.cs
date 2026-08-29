using Microsoft.AspNetCore.Mvc;
using PPFarmWA.BD.Datos.Entity;
using PPFarmWA.Repositorio.Repositorios;
using PPFarmWA.Shared.DTO;

namespace PPFarmWA.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecursoController : ControllerBase
    {
        private readonly IRecursoRepositorio _repositorio;

        public RecursoController(IRecursoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecursoDTO>>> Get()
        {
            var recursos = await _repositorio.GetAllAsync();

            var resultado = recursos.Select(r => new RecursoDTO
            {
                Id = r.Id,
                nombre = r.nombre,
                descripcion = r.descripcion,
                eficiencia = r.eficiencia,
                durabilidad = r.durabilidad,
                valor = r.valor,
                deTienda = r.deTienda,
                tipo = r.tipo,
                idRareza = r.idRareza
            });

            return Ok(resultado);
        }

        [HttpGet("tienda")]
        public async Task<ActionResult<IEnumerable<RecursoDTO>>> GetTienda()
        {
            var recursos = await _repositorio.GetRecursosTiendaAsync();

            var resultado = recursos.Select(r => new RecursoDTO
            {
                Id = r.Id,
                nombre = r.nombre,
                descripcion = r.descripcion,
                eficiencia = r.eficiencia,
                durabilidad = r.durabilidad,
                valor = r.valor,
                deTienda = r.deTienda,
                tipo = r.tipo,
                idRareza = r.idRareza
            });

            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecursoDTO>> GetById(int id)
        {
            var recurso = await _repositorio.GetByIdAsync(id);

            if (recurso == null)
                return NotFound();

            var dto = new RecursoDTO
            {
                Id = recurso.Id,
                nombre = recurso.nombre,
                descripcion = recurso.descripcion,
                eficiencia = recurso.eficiencia,
                durabilidad = recurso.durabilidad,
                valor = recurso.valor,
                deTienda = recurso.deTienda,
                tipo = recurso.tipo,
                idRareza = recurso.idRareza
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<RecursoDTO>> Post(RecursoDTO dto)
        {
            var recurso = new Recurso
            {
                nombre = dto.nombre,
                descripcion = dto.descripcion,
                eficiencia = dto.eficiencia,
                durabilidad = dto.durabilidad,
                valor = dto.valor,
                deTienda = dto.deTienda,
                tipo = dto.tipo,
                idRareza = dto.idRareza
            };

            var creado = await _repositorio.AddAsync(recurso);

            dto.Id = creado.Id;

            return CreatedAtAction(
                nameof(GetById),
                new { id = creado.Id },
                dto
            );
        }
    }
}
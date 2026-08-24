using Microsoft.AspNetCore.Mvc;
using PPFarmWA.Repositorio.Repositorios;
using PPFarmWA.Shared.DTO;

namespace PPFarmWA.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JugadorController : ControllerBase
    {
        private readonly IJugadorRepositorio _repositorio;

        public JugadorController(IJugadorRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JugadorDTO>>> Get()
        {
            var jugadores = await _repositorio.GetAllAsync();

            var resultado = jugadores.Select(j => new JugadorDTO
            {
                Id = j.Id,
                userName = j.userName,
                email = j.email,
                ppCoins = j.ppCoins,
                points = j.points,
                level = j.level,
                experiencia = j.experiencia,
                esTienda = j.esTienda,
                esAdmin = j.esAdmin,
                idUltimaHerramienta = j.idUltimaHerramienta,
                idUltimoDispositivo = j.idUltimoDispositivo,
                idUltimoPotenciador = j.idUltimoPotenciador
            });

            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JugadorDTO>> GetById(int id)
        {
            var jugador = await _repositorio.GetByIdAsync(id);

            if (jugador == null)
                return NotFound();

            var dto = new JugadorDTO
            {
                Id = jugador.Id,
                userName = jugador.userName,
                email = jugador.email,
                ppCoins = jugador.ppCoins,
                points = jugador.points,
                level = jugador.level,
                experiencia = jugador.experiencia,
                esTienda = jugador.esTienda,
                esAdmin = jugador.esAdmin,
                idUltimaHerramienta = jugador.idUltimaHerramienta,
                idUltimoDispositivo = jugador.idUltimoDispositivo,
                idUltimoPotenciador = jugador.idUltimoPotenciador
            };

            return Ok(dto);
        }

        [HttpPut("{id}/coins")]
        public async Task<IActionResult> ModificarCoins(
            int id,
            [FromQuery] double cantidad)
        {
            var resultado = await _repositorio.ModificarCoinsAsync(id, cantidad);

            if (!resultado)
                return NotFound();

            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPFarmWA.BD.Datos.Entity;
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

            return Ok(jugadores.Select(Proyectar));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JugadorDTO>> GetById(int id)
        {
            var jugador = await _repositorio.GetByIdAsync(id);

            if (jugador == null)
                return NotFound();

            return Ok(Proyectar(jugador));
        }

        [HttpPost]
        public async Task<ActionResult<JugadorDTO>> Post(JugadorDTO dto)
        {
            var jugador = new Jugador
            {
                userName = dto.userName,
                password = dto.password,
                email = dto.email,
                ppCoins = dto.ppCoins,
                points = dto.points,
                level = dto.level,
                experiencia = dto.experiencia,
                esTienda = dto.esTienda,
                esAdmin = dto.esAdmin,
                idUltimaHerramienta = dto.idUltimaHerramienta,
                idUltimoDispositivo = dto.idUltimoDispositivo,
                idUltimoPotenciador = dto.idUltimoPotenciador
            };

            var creado = await _repositorio.AddAsync(jugador);

            dto.Id = creado.Id;

            return CreatedAtAction(
                nameof(GetById),
                new { id = creado.Id },
                dto
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<JugadorDTO>> Actualizar(int id, JugadorDTO dto)
        {
            var jugador = await _repositorio.GetByIdAsync(id);

            if (jugador == null)
                return NotFound();

            // Solo se persisten los datos de partida.
            // userName, email y password son identidad: no se tocan desde acá.
            jugador.level = dto.level;
            jugador.experiencia = dto.experiencia;
            jugador.ppCoins = dto.ppCoins;
            jugador.points = dto.points;
            jugador.idUltimaHerramienta = dto.idUltimaHerramienta;
            jugador.idUltimoDispositivo = dto.idUltimoDispositivo;
            jugador.idUltimoPotenciador = dto.idUltimoPotenciador;

            await _repositorio.UpdateAsync(jugador);

            return Ok(Proyectar(jugador));
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

        [HttpPost("registrar")]
        public async Task<ActionResult<SesionDTO>> RegistrarJugador(RegistroDTO dto)
        {

            if (await _repositorio.ExisteEmailOusuarioAsync(dto.userName) || await _repositorio.ExisteEmailOusuarioAsync(dto.email))
            {
                return BadRequest("El email o el nombre de usuario ya están en uso.");
            }
            var sesion = await _repositorio.RegistrarJugadorAsync(dto);
            return Ok(sesion);
        }

        [HttpPost("login")]
        public async Task<ActionResult<SesionDTO>> ObtenerSesionConItems(LoginDTO dto)
        {
            var sesion = await _repositorio.ObtenerSesionConItemsAsync(dto.JugadorId);
            if (sesion == null)
            {
                return NotFound();
            }
            return Ok(sesion);
        }
        [HttpGet ("existe-email-usuario")]
        public async Task<ActionResult<bool>> ExisteEmailOusuario([FromQuery] string emailOusuario)
        {
            var existe = await _repositorio.ExisteEmailOusuarioAsync(emailOusuario);
            return Ok(existe);
        }

        private static JugadorDTO Proyectar(Jugador j)
        {
            return new JugadorDTO
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
            };
        }

    }
}
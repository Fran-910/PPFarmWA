
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
        public async Task<ActionResult<JugadorPutDTO>> Actualizar(int id, JugadorPutDTO dto)
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
        [HttpPost("login1")]
        public async Task<ActionResult<SesionDTO>> ObtenerSesionConItemsPorEmailAsync([FromBody] Login1DTO dto)
        {
            // 1. Validar que el DTO o el Username no vengan vacíos
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest("El mail es obligatorio.");
            }

            // 2. Consultar el repositorio por email
            var sesion = await _repositorio.ObtenerSesionConItemsPorEmailAsync(dto.Email);

            // 3. Si no existe el email
            if (sesion == null)
            {
                return NotFound("Usuario o contraseña no válidos.");
            }

            return Ok(sesion);
        }
        [HttpPost("loginmailpass")]
        public async Task<ActionResult<SesionDTO>> ObtenerSesionConItemsPorEmailYPasswordAsync([FromBody] Login1DTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("El correo electrónico y la contraseña son obligatorios.");
            }

            var sesion = await _repositorio.ObtenerSesionConItemsPorEmailYPasswordAsync(dto.Email, dto.Password);

            if (sesion == null)
            {
                return NotFound("Correo electrónico o contraseña incorrectos.");
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
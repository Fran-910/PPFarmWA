
using Microsoft.AspNetCore.Mvc;
using PPFarmWA.BD.Datos.Entity;
using PPFarmWA.Repositorio.Repositorios;
using PPFarmWA.Shared.DTO;
using PPFarmWA.Server.Services;

namespace PPFarmWA.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JugadorController : ControllerBase
    {
        private readonly IJugadorRepositorio _repositorio;
        private readonly ConversorServicio _conversorServicio;

        public JugadorController(
            IJugadorRepositorio repositorio,
            ConversorServicio conversorServicio)
        {
            _repositorio = repositorio;
            _conversorServicio = conversorServicio;
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

                // Los saldos iniciales siempre se establecen desde el servidor.
                ppCoins = 0,
                points = 0,

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
                Proyectar(creado));
        }

        // Actualiza el progreso y el inventario.
        // Los saldos se modifican mediante operaciones específicas del servidor.
        [HttpPut("{id}")]
        public async Task<ActionResult<JugadorDTO>> Actualizar(
            int id,
            JugadorPutDTO dto)
        {
            var jugador = await _repositorio.GetByIdAsync(id);

            if (jugador == null)
                return NotFound();

            var jugadorInv = await _repositorio.GetPlayerInv(id);

            await _repositorio.UpdatePlayerInvAsync(
                dto.items,
                jugadorInv);

            jugador.level = dto.level;
            jugador.experiencia = dto.experiencia;
            jugador.idUltimaHerramienta = dto.idUltimaHerramienta;

            await _repositorio.UpdateAsync(jugador);

            return Ok(Proyectar(jugador));
        }

        [HttpPut("{id}/coins")]
        public async Task<IActionResult> ModificarCoins(
            int id,
            [FromQuery] double cantidad)
        {
            var resultado =
                await _repositorio.ModificarCoinsAsync(id, cantidad);

            if (!resultado)
                return BadRequest(new
                {
                    Mensaje = "No se pudieron modificar los PP Coins. Verificá el saldo y el límite global."
                });

            return Ok(new
            {
                Mensaje = "PP Coins actualizados correctamente."
            });
        }

        [HttpPost("{id}/recompensa-points")]
        public async Task<IActionResult> AcreditarPointsPorCosecha(int id)
        {
            var resultado =
                await _repositorio.AcreditarPointsPorCosechaAsync(id);

            if (!resultado.Exito)
            {
                return BadRequest(new
                {
                    Mensaje = resultado.Mensaje,
                    Points = resultado.Points,
                    Recompensa = resultado.Recompensa
                });
            }

            return Ok(new
            {
                Mensaje = resultado.Mensaje,
                Points = resultado.Points,
                Recompensa = resultado.Recompensa
            });
        }

        [HttpPut("{id}/convertir-points")]
        public async Task<IActionResult> ConvertirPoints(
            int id,
            [FromQuery] int puntos)
        {
            var resultado =
                await _conversorServicio.ConvertirPointsAsync(id, puntos);

            if (!resultado.Exito)
            {
                return BadRequest(new
                {
                    Mensaje = resultado.Mensaje
                });
            }

            return Ok(new
            {
                Mensaje = resultado.Mensaje
            });
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<SesionDTO>> RegistrarJugador(
            RegistroDTO dto)
        {
            if (await _repositorio.ExisteEmailOusuarioAsync(dto.userName) ||
                await _repositorio.ExisteEmailOusuarioAsync(dto.email))
            {
                return BadRequest(
                    "El email o el nombre de usuario ya están en uso.");
            }

            var sesion =
                await _repositorio.RegistrarJugadorAsync(dto);

            return Ok(sesion);
        }

        [HttpPost("login")]
        public async Task<ActionResult<SesionDTO>> ObtenerSesionConItems(
            LoginDTO dto)
        {
            var sesion =
                await _repositorio.ObtenerSesionConItemsAsync(dto.JugadorId);

            if (sesion == null)
                return NotFound();

            return Ok(sesion);
        }

        [HttpPost("login1")]
        public async Task<ActionResult<SesionDTO>>
            ObtenerSesionConItemsPorEmailAsync(
                [FromBody] Login1DTO dto)
        {
            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest("El mail es obligatorio.");
            }

            var sesion =
                await _repositorio.ObtenerSesionConItemsPorEmailAsync(
                    dto.Email);

            if (sesion == null)
                return NotFound("Usuario o contraseña no válidos.");

            return Ok(sesion);
        }

        [HttpPost("loginmailpass")]
        public async Task<ActionResult<SesionDTO>>
            ObtenerSesionConItemsPorEmailYPasswordAsync(
                [FromBody] Login1DTO dto)
        {
            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(
                    "El correo electrónico y la contraseña son obligatorios.");
            }

            var sesion =
                await _repositorio.ObtenerSesionConItemsPorEmailYPasswordAsync(
                    dto.Email,
                    dto.Password);

            if (sesion == null)
                return NotFound(
                    "Correo electrónico o contraseña incorrectos.");

            return Ok(sesion);
        }

        [HttpGet("existe-email-usuario")]
        public async Task<ActionResult<bool>> ExisteEmailOusuario(
            [FromQuery] string emailOusuario)
        {
            var existe =
                await _repositorio.ExisteEmailOusuarioAsync(emailOusuario);

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

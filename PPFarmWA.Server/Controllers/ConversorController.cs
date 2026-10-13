using Microsoft.AspNetCore.Mvc;
using PPFarmWA.Server.Services;
using System.Threading.Tasks;

namespace PPFarmWA.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversorController : ControllerBase
    {
        private readonly ConversorServicio _conversorServicio;

        public ConversorController(ConversorServicio conversorServicio)
        {
            _conversorServicio = conversorServicio;
        }

        [HttpPost("convertir")]
        public async Task<IActionResult> Convertir([FromBody] ConversionRequestDTO request)
        {
            // Llamamos al servicio con los 2 argumentos exactos
            var resultado = await _conversorServicio.ConvertirPointsAsync(request.IdJugador, request.PuntosAConvertir);

            if (!resultado.Exito)
            {
                // Si falla (ej: points insuficientes), devolvemos un HTTP 400 (Bad Request)
                return BadRequest(new { mensaje = resultado.Mensaje });
            }

            // Si sale bien, devolvemos un HTTP 200 (Ok)
            return Ok(new { mensaje = resultado.Mensaje });
        }
    }

    // El DTO limpio, solo con los datos que necesitamos
    public class ConversionRequestDTO
    {
        public int IdJugador { get; set; }
        public int PuntosAConvertir { get; set; }
    }
}
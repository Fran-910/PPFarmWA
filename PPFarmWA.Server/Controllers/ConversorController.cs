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
            
            var resultado = await _conversorServicio.ConvertirPointsAsync(request.IdJugador, request.PuntosAConvertir);

            if (!resultado.Exito)
            {
                
                return BadRequest(new { mensaje = resultado.Mensaje });
            }

            
            return Ok(new { mensaje = resultado.Mensaje });
        }
    }

    
    public class ConversionRequestDTO
    {
        public int IdJugador { get; set; }
        public int PuntosAConvertir { get; set; }
    }
}
using Microsoft.AspNetCore.Mvc;
using PPFarmWA.Servicios;
using PPFarmWA.BD.Datos.Entity;

namespace PPFarmWA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MercadoController : ControllerBase
    {
        private readonly MercadoServicio _mercadoServicio;

        public MercadoController(MercadoServicio mercadoServicio)
        {
            _mercadoServicio = mercadoServicio;
        }

        [HttpGet("listar/{idJugador}")]
        public async Task<ActionResult<List<Venta>>> Listar(int idJugador)
        {
            var activas = await _mercadoServicio.ListarActivas(idJugador);
            return Ok(activas);
        }

        [HttpPost("publicar")]
        public async Task<IActionResult> Publicar([FromBody] PublicarRequest request)
        {
            var resultado = await _mercadoServicio.PublicarItem(request.IdJugador, request.IdItem, request.Precio);
            if (!resultado.Exito) return BadRequest(resultado.Mensaje);
            return Ok(resultado.Mensaje);
        }

        [HttpPost("comprar")]
        public async Task<IActionResult> Comprar([FromBody] ComprarRequest request)
        {
            var resultado = await _mercadoServicio.Comprar(request.IdVenta, request.IdComprador);

            // CRITERIO DE ACEPTACIÓN: Si dos compran a la vez, uno recibe 409 Conflict.
            if (resultado.Conflicto) return StatusCode(409, resultado.Mensaje);

            if (!resultado.Exito) return BadRequest(resultado.Mensaje);

            return Ok(resultado.Mensaje);
        }
    }

    // Clases auxiliares para recibir los datos del frontend
    public class PublicarRequest
    {
        public int IdJugador { get; set; }
        public int IdItem { get; set; }
        public double Precio { get; set; }
    }

    public class ComprarRequest
    {
        public int IdVenta { get; set; }
        public int IdComprador { get; set; }
    }
}
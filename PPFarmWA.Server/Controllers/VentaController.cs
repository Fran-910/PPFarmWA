using Microsoft.AspNetCore.Mvc;
using PPFarmWA.BD.Datos.Entity;
using PPFarmWA.Repositorio.Repositorios;
using PPFarmWA.Shared.DTO;

namespace PPFarmWA.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly IVentaRepositorio _repositorio;

        public VentaController(IVentaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaDTO>>> Get()
        {
            var ventas = await _repositorio.GetAllAsync();

            var resultado = ventas.Select(v => new VentaDTO
            {
                Id = v.Id,
                idJugadorVendedor = v.idJugadorVendedor,
                idJugadorComprador = v.idJugadorComprador,
                cantidadVenta = v.cantidadVenta,
                precioVenta = v.precioVenta
            });

            return Ok(resultado);
        }

        [HttpGet("jugador/{idJugador}")]
        public async Task<ActionResult<IEnumerable<VentaDTO>>> GetVentasJugador(int idJugador)
        {
            var ventas = await _repositorio.GetVentasJugadorAsync(idJugador);

            var resultado = ventas.Select(v => new VentaDTO
            {
                Id = v.Id,
                idJugadorVendedor = v.idJugadorVendedor,
                idJugadorComprador = v.idJugadorComprador,
                cantidadVenta = v.cantidadVenta,
                precioVenta = v.precioVenta
            });

            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VentaDTO>> GetById(int id)
        {
            var venta = await _repositorio.GetByIdAsync(id);

            if (venta == null)
                return NotFound();

            var dto = new VentaDTO
            {
                Id = venta.Id,
                idJugadorVendedor = venta.idJugadorVendedor,
                idJugadorComprador = venta.idJugadorComprador,
                cantidadVenta = venta.cantidadVenta,
                precioVenta = venta.precioVenta
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<VentaDTO>> Post(VentaDTO dto)
        {
            var venta = new Venta
            {
                idJugadorVendedor = dto.idJugadorVendedor,
                idJugadorComprador = dto.idJugadorComprador,
                cantidadVenta = dto.cantidadVenta,
                precioVenta = dto.precioVenta
            };

            var creado = await _repositorio.AddAsync(venta);

            dto.Id = creado.Id;

            return CreatedAtAction(
                nameof(GetById),
                new { id = creado.Id },
                dto
            );
        }
    }
}
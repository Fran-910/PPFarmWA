using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPFarmWA.BD.Datos.Entity;
using PPFarmWA.Repositorio.Repositorios;
using PPFarmWA.Shared.DTO;
using PPFarmWA.BD.Datos;

namespace PPFarmWA.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly IVentaRepositorio _repositorio;
        private readonly IItemRepositorio _itemRepositorio;
        private readonly AppDbContext _context;

        public VentaController(
            IVentaRepositorio repositorio,
            IItemRepositorio itemRepositorio,
            AppDbContext context)
        {
            _repositorio = repositorio;
            _itemRepositorio = itemRepositorio;
            _context = context;
        }

        [HttpPost("{id:int}/comprar")]
        public async Task<IActionResult> Comprar(int id, int idJugadorComprador)
        {
            await using var transaccion = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Buscar la publicación
                var venta = await _context.Ventas.FindAsync(id);

                if (venta == null)
                    return NotFound("La publicación no existe.");

                // 2. Verificar que todavía esté disponible
                if (venta.idJugadorComprador != null)
                    return BadRequest("Esta publicación ya fue comprada.");

                // 3. Evitar que el vendedor se compre a sí mismo
                if (venta.idJugadorVendedor == idJugadorComprador)
                    return BadRequest("No podés comprar tu propia publicación.");

                // 4. Buscar vendedor y comprador
                var vendedor = await _context.Jugadores.FindAsync(venta.idJugadorVendedor);
                var comprador = await _context.Jugadores.FindAsync(idJugadorComprador);

                if (vendedor == null)
                    return NotFound("El vendedor no existe.");

                if (comprador == null)
                    return NotFound("El comprador no existe.");

                // 5. Buscar el item publicado
                var itemVendedor = await _context.Items
                    .FirstOrDefaultAsync(i => i.VentaId == venta.Id);

                if (itemVendedor == null)
                    return NotFound("No se encontró el recurso publicado.");

                // 6. Verificar que el vendedor todavía tenga la cantidad
                if (itemVendedor.cantidad < venta.cantidadVenta)
                    return BadRequest("El vendedor no tiene suficiente cantidad.");

                Console.WriteLine($"COMPRADOR {comprador.Id} - COINS ANTES: {comprador.ppCoins}");
                Console.WriteLine($"VENDEDOR {vendedor.Id} - COINS ANTES: {vendedor.ppCoins}");
                Console.WriteLine($"PRECIO VENTA: {venta.precioVenta}");

                // 7. Verificar PP Coins
                if (comprador.ppCoins < venta.precioVenta)
                    return BadRequest("No tenés suficientes PP Coins.");

                // 8. Buscar si el comprador ya tiene ese recurso
                var itemComprador = await _context.Items
                    .FirstOrDefaultAsync(i =>
                        i.JugadorId == comprador.Id &&
                        i.RecursoId == itemVendedor.RecursoId);

                // 9. Transferir el recurso
                itemVendedor.cantidad -= venta.cantidadVenta;

                if (itemComprador == null)
                {

                    itemComprador = new Item
                    {
                        cantidad = venta.cantidadVenta,
                        JugadorId = comprador.Id,
                        RecursoId = itemVendedor.RecursoId
                    };

                    _context.Items.Add(itemComprador);
                }
                else
                {
                    itemComprador.cantidad += venta.cantidadVenta;
                }

                _context.Items.Remove(itemVendedor);

                // 10. Transferir PP Coins
                comprador.ppCoins -= venta.precioVenta;
                vendedor.ppCoins += venta.precioVenta;

                // 11. Marcar la publicación como comprada
                venta.idJugadorComprador = comprador.Id;

               
                // 12. Guardar cambios
                await _context.SaveChangesAsync();
                await transaccion.CommitAsync();

                return Ok(new
                {
                    mensaje = "Compra realizada correctamente."
                });
            }
            catch
            {
                await transaccion.RollbackAsync();
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "No se pudo completar la compra."
                );
            }
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



        [HttpGet("disponibles")]
        public async Task<ActionResult<IEnumerable<VentaDTO>>> GetDisponibles()
        {
            var ventas = await _repositorio.GetVentasDisponiblesAsync();

            var resultado = new List<VentaDTO>();

            foreach (var v in ventas)
            {
                var item = await _context.Items
                    .FirstOrDefaultAsync(i => i.VentaId == v.Id);

                if (item == null)
                    continue;

                var recurso = await _context.Recursos
                    .FindAsync(item.RecursoId);

                resultado.Add(new VentaDTO
                {
                    Id = v.Id,
                    idJugadorVendedor = v.idJugadorVendedor,
                    idJugadorComprador = v.idJugadorComprador,
                    cantidadVenta = v.cantidadVenta,
                    precioVenta = v.precioVenta,
                    idRecurso = item.RecursoId,
                    nombreRecurso = recurso?.nombre
                });
            }

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

        [HttpPost("publicar/{itemId:int}")]
        public async Task<ActionResult<VentaDTO>> Publicar(int itemId, VentaDTO dto)
        {
            var item = await _itemRepositorio.GetByIdAsync(itemId);

            if (item == null)
                return NotFound("El item no existe.");

            if (item.JugadorId != dto.idJugadorVendedor)
                return BadRequest("El item no pertenece al jugador.");

            if (dto.cantidadVenta <= 0)
                return BadRequest("La cantidad debe ser mayor a 0.");

            if (dto.cantidadVenta > item.cantidad)
                return BadRequest("No tenés suficiente cantidad de ese item.");

            if (item.VentaId != null)
                return BadRequest("Este item ya está publicado.");

            var venta = new Venta
            {
                idJugadorVendedor = dto.idJugadorVendedor,
                idJugadorComprador = null,
                cantidadVenta = dto.cantidadVenta,
                precioVenta = dto.precioVenta
            };

            var creado = await _repositorio.AddAsync(venta);

            // Descontamos la cantidad publicada del inventario
            item.cantidad -= dto.cantidadVenta;

            await _itemRepositorio.UpdateAsync(item);

            // Creamos el item reservado para la publicación
            var itemPublicado = new Item
            {
                cantidad = dto.cantidadVenta,
                JugadorId = item.JugadorId,
                RecursoId = item.RecursoId,
                VentaId = creado.Id
            };

            await _itemRepositorio.AddAsync(itemPublicado);

            dto.Id = creado.Id;

            return CreatedAtAction(
                nameof(GetById),
                new { id = creado.Id },
                dto
            );
        }


    }
    
}
    



using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PPFarmWA.BD.Datos;
using PPFarmWA.BD.Datos.Entity;
using System.Data;

namespace PPFarmWA.Servicios
{
    public class MercadoServicio
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public MercadoServicio(
            AppDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Listar publicaciones activas excluyendo las propias.
        public async Task<List<Venta>> ListarActivas(int idJugadorActual)
        {
            return await _context.Ventas
                .Where(v =>
                    v.idJugadorComprador == null &&
                    v.idJugadorVendedor != idJugadorActual)
                .ToListAsync();
        }

        // Publicar un ítem en el mercado.
        public async Task<(bool Exito, string Mensaje)> PublicarItem(
            int idJugador,
            int idItem,
            double precio)
        {
            if (idJugador <= 0 ||
                idItem <= 0 ||
                !double.IsFinite(precio) ||
                precio <= 0)
            {
                return (false, "Los datos de publicación no son válidos.");
            }

            var item = await _context.Items.FirstOrDefaultAsync(
                i => i.Id == idItem && i.JugadorId == idJugador);

            if (item == null)
                return (false, "El ítem no existe o no te pertenece.");

            if (item.VentaId != null)
                return (false, "Este ítem ya está publicado en el mercado.");

            var nuevaVenta = new Venta
            {
                idJugadorVendedor = idJugador,
                precioVenta = precio,
                cantidadVenta = item.cantidad
            };

            await using var transaction =
                await _context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            try
            {
                _context.Ventas.Add(nuevaVenta);

                await _context.SaveChangesAsync();

                item.VentaId = nuevaVenta.Id;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return (true, "Publicación creada con éxito.");
            }
            catch
            {
                await transaction.RollbackAsync();

                return (false, "No se pudo publicar el ítem.");
            }
        }

        // Comprar publicación de forma transaccional.
        public async Task<(bool Exito, string Mensaje, bool Conflicto)> Comprar(
            int idVenta,
            int idComprador)
        {
            if (idVenta <= 0 || idComprador <= 0)
            {
                return (
                    false,
                    "Los datos de compra no son válidos.",
                    false);
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            try
            {
                var venta = await _context.Ventas
                    .FirstOrDefaultAsync(v => v.Id == idVenta);

                if (venta == null)
                {
                    await transaction.RollbackAsync();
                    return (false, "La publicación no existe.", false);
                }

                if (venta.idJugadorComprador != null)
                {
                    await transaction.RollbackAsync();

                    return (
                        false,
                        "Esta publicación ya fue comprada por otro jugador.",
                        true);
                }

                if (venta.idJugadorVendedor == idComprador)
                {
                    await transaction.RollbackAsync();

                    return (
                        false,
                        "No puedes comprar tu propia publicación.",
                        false);
                }

                if (!double.IsFinite(venta.precioVenta) ||
                    venta.precioVenta <= 0)
                {
                    await transaction.RollbackAsync();

                    return (
                        false,
                        "El precio de la publicación no es válido.",
                        false);
                }

                var comprador = await _context.Jugadores
                    .FirstOrDefaultAsync(j => j.Id == idComprador);

                var vendedor = await _context.Jugadores
                    .FirstOrDefaultAsync(
                        j => j.Id == venta.idJugadorVendedor);

                var item = await _context.Items
                    .FirstOrDefaultAsync(i => i.VentaId == idVenta);

                if (comprador == null ||
                    vendedor == null ||
                    item == null)
                {
                    await transaction.RollbackAsync();

                    return (
                        false,
                        "Error de integridad de datos.",
                        false);
                }

                // Validar que los saldos sean números válidos.
                if (!double.IsFinite(comprador.ppCoins) ||
                    !double.IsFinite(vendedor.ppCoins) ||
                    comprador.ppCoins < 0 ||
                    vendedor.ppCoins < 0)
                {
                    await transaction.RollbackAsync();

                    return (
                        false,
                        "Los saldos de PP Coins no son válidos.",
                        false);
                }

                if (comprador.ppCoins < venta.precioVenta)
                {
                    await transaction.RollbackAsync();

                    return (
                        false,
                        "No tienes suficientes PP Coins.",
                        false);
                }

                double limiteGlobal = _configuration.GetValue<double>(
                    "EconomiaJuego:LimiteGlobalPPCoins");

                if (!double.IsFinite(limiteGlobal) ||
                    limiteGlobal <= 0)
                {
                    await transaction.RollbackAsync();

                    return (
                        false,
                        "La configuración del límite global no es válida.",
                        false);
                }

                double totalCoins = await _context.Jugadores
                    .SumAsync(j => j.ppCoins);

                if (!double.IsFinite(totalCoins) ||
                    totalCoins < 0 ||
                    totalCoins > limiteGlobal)
                {
                    await transaction.RollbackAsync();

                    return (
                        false,
                        "El total global de PP Coins no respeta el límite permitido.",
                        false);
                }

                double nuevoSaldoComprador =
                    comprador.ppCoins - venta.precioVenta;

                double nuevoSaldoVendedor =
                    vendedor.ppCoins + venta.precioVenta;

                // La compra transfiere monedas: no crea monedas nuevas.
                if (!double.IsFinite(nuevoSaldoComprador) ||
                    !double.IsFinite(nuevoSaldoVendedor) ||
                    nuevoSaldoComprador < 0)
                {
                    await transaction.RollbackAsync();

                    return (
                        false,
                        "La operación produciría saldos inválidos.",
                        false);
                }

                comprador.ppCoins = nuevoSaldoComprador;
                vendedor.ppCoins = nuevoSaldoVendedor;

                venta.idJugadorComprador = idComprador;

                item.JugadorId = idComprador;
                item.VentaId = null;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "Compra realizada con éxito.", false);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                if (ex is DbUpdateConcurrencyException ||
                    ex.Message.Contains(
                        "deadlock",
                        StringComparison.OrdinalIgnoreCase) ||
                    ex.Message.Contains(
                        "transaction",
                        StringComparison.OrdinalIgnoreCase))
                {
                    return (
                        false,
                        "Conflicto de concurrencia: alguien más intentó comprar esto al mismo tiempo.",
                        true);
                }

                return (
                    false,
                    "Error interno al realizar la compra.",
                    false);
            }
        }
    }
}

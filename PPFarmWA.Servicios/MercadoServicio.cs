using Microsoft.EntityFrameworkCore;
using PPFarmWA.BD.Datos;
using PPFarmWA.BD.Datos.Entity;
using System.Data;

namespace PPFarmWA.Servicios
{
    public class MercadoServicio
    {
        private readonly AppDbContext _context;

        public MercadoServicio(AppDbContext context)
        {
            _context = context;
        }

        // Listar publicaciones activas excluyendo las propias
        public async Task<List<Venta>> ListarActivas(int idJugadorActual)
        {
            return await _context.Ventas
                .Where(v => v.idJugadorComprador == null && v.idJugadorVendedor != idJugadorActual)
                .ToListAsync();
        }

        //  Publicar un ítem en el mercado
        public async Task<(bool Exito, string Mensaje)> PublicarItem(int idJugador, int idItem, double precio)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == idItem && i.JugadorId == idJugador);
            if (item == null) return (false, "El ítem no existe o no te pertenece.");
            if (item.VentaId != null) return (false, "Este ítem ya está publicado en el mercado.");

            // Creacion de la venta
            var nuevaVenta = new Venta
            {
                idJugadorVendedor = idJugador,
                precioVenta = precio,
                cantidadVenta = item.cantidad
            };

            _context.Ventas.Add(nuevaVenta);
            await _context.SaveChangesAsync(); 

            
            item.VentaId = nuevaVenta.Id;
            await _context.SaveChangesAsync();

            return (true, "Publicación creada con éxito.");
        }

        // 3. Comprar publicación de forma transaccional y manejando concurrencia
        public async Task<(bool Exito, string Mensaje, bool Conflicto)> Comprar(int idVenta, int idComprador)
        {
           
            using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            try
            {
                var venta = await _context.Ventas.FirstOrDefaultAsync(v => v.Id == idVenta);
                if (venta == null) return (false, "La publicación no existe.", false);

               
                if (venta.idJugadorComprador != null)
                {
                    return (false, "Esta publicación ya fue comprada por otro jugador.", true); // true = Conflicto 409
                }

                if (venta.idJugadorVendedor == idComprador)
                    return (false, "No puedes comprar tu propia publicación.", false);

                var comprador = await _context.Jugadores.FindAsync(idComprador);
                var vendedor = await _context.Jugadores.FindAsync(venta.idJugadorVendedor);
                var item = await _context.Items.FirstOrDefaultAsync(i => i.VentaId == idVenta);

                if (comprador == null || vendedor == null || item == null)
                    return (false, "Error de integridad de datos.", false);

                if (comprador.ppCoins < venta.precioVenta)
                    return (false, "No tienes suficientes PP Coins.", false);

                comprador.ppCoins -= venta.precioVenta;
                vendedor.ppCoins += venta.precioVenta;

                
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

                
                if (ex is DbUpdateConcurrencyException || ex.Message.Contains("deadlock") || ex.Message.Contains("transaction"))
                {
                    return (false, "Conflicto de concurrencia: Alguien más intentó comprar esto al mismo tiempo.", true);
                }

                return (false, "Error interno.", false);
            }
        }
    }
}
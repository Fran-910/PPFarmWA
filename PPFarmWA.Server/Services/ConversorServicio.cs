using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PPFarmWA.BD.Datos;

namespace PPFarmWA.Server.Services // Cambia esto por el namespace de tu backend
{
    public class ConversorServicio
    {
        private readonly AppDbContext _context; // Reemplaza con el nombre de tu DbContext
        private readonly IConfiguration _config;

        public ConversorServicio(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // El Guid requestId es vital para que no te hagan doble clic y dupliquen monedas
        public async Task<(bool Exito, string Mensaje)> ConvertirPointsAsync(int idJugador, int puntosAConvertir)
        {
            // Iniciamos la transacción (Todo o nada)
            using var transaccion = await _context.Database.BeginTransactionAsync();

            try
            {
                // Buscar al jugador en la BD
                var jugador = await _context.Jugadores.FindAsync(idJugador);
                if (jugador == null) return (false, "Jugador no encontrado.");

                // Validar saldos (Points insuficientes -> frena todo)
                // ATENCIÓN: Cambiá "points" por el nombre exacto que tenga tu clase Jugador
                if (jugador.points < puntosAConvertir)
                {
                    return (false, "Points insuficientes.");
                }

                // Leer la tasa configurable del appsettings.json
                decimal tasa = _config.GetValue<decimal>("EconomiaJuego:TasaConversion");
                int equivalenciaCoins = (int)(puntosAConvertir / tasa);

                // Descontar y acreditar (Cambiá "points" y "ppCoins" por los nombres exactos)
                jugador.points -= puntosAConvertir;
                jugador.ppCoins += equivalenciaCoins;

                // Guardar cambios en la base de datos
                await _context.SaveChangesAsync();

                // Confirmar la transacción (Commit)
                await transaccion.CommitAsync();

                return (true, $"Conversión exitosa. Recibiste {equivalenciaCoins} PP Coins.");
            }
            catch (Exception)
            {
                // Si algo falla, deshacemos todo
                await transaccion.RollbackAsync();
                return (false, "Error interno al procesar la conversión.");
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PPFarmWA.BD.Datos;

namespace PPFarmWA.Server.Services 
{
    public class ConversorServicio
    {
        private readonly AppDbContext _context; 
        private readonly IConfiguration _config;

        public ConversorServicio(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        
        public async Task<(bool Exito, string Mensaje)> ConvertirPointsAsync(int idJugador, int puntosAConvertir)
        {
            
            using var transaccion = await _context.Database.BeginTransactionAsync();

            try
            {
                
                var jugador = await _context.Jugadores.FindAsync(idJugador);
                if (jugador == null) return (false, "Jugador no encontrado.");

                
                if (jugador.points < puntosAConvertir)
                {
                    return (false, "Points insuficientes.");
                }

                
                decimal tasa = _config.GetValue<decimal>("EconomiaJuego:TasaConversion");
                int equivalenciaCoins = (int)(puntosAConvertir / tasa);

                
                jugador.points -= puntosAConvertir;
                jugador.ppCoins += equivalenciaCoins;

                
                await _context.SaveChangesAsync();

               
                await transaccion.CommitAsync();

                return (true, $"Conversión exitosa. Recibiste {equivalenciaCoins} PP Coins.");
            }
            catch (Exception)
            {
                await transaccion.RollbackAsync();
                return (false, "Error interno al procesar la conversión.");
            }
        }
    }
}
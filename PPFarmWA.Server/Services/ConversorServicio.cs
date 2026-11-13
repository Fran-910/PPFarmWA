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
            
            using var transaccion = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

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

                
                double limiteGlobal = _config.GetValue<double>("EconomiaJuego:LimiteGlobalPPCoins");
                double totalMonedasActuales = await _context.Jugadores.SumAsync(j => j.ppCoins);

                if (totalMonedasActuales + equivalenciaCoins > limiteGlobal)
                {
                    await transaccion.RollbackAsync();
                    return (false, "El Banco Central ha alcanzado el límite global de PP Coins. Intente más tarde.");
                }
                

                jugador.points -= puntosAConvertir;
                jugador.ppCoins += equivalenciaCoins;

                await _context.SaveChangesAsync();
                await transaccion.CommitAsync();

                return (true, $"Conversión exitosa. Recibiste {equivalenciaCoins} PP Coins.");
            }
            catch (Exception ex)
            {
                await transaccion.RollbackAsync();
                
                return (false, $"Error interno: {ex.Message}");
            }
        }
    }
}
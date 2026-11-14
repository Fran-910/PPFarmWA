
using Microsoft.EntityFrameworkCore;
using PPFarmWA.BD.Datos;
using System.Data;

namespace PPFarmWA.Server.Services
{
    public class ConversorServicio
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ConversorServicio(
            AppDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<(bool Exito, string Mensaje)> ConvertirPointsAsync(
            int idJugador,
            int puntosAConvertir)
        {
            if (idJugador <= 0)
                return (false, "El jugador no es válido.");

            if (puntosAConvertir <= 0)
                return (false, "Debes ingresar una cantidad válida de Points.");

            decimal tasa = _configuration.GetValue<decimal>(
                "EconomiaJuego:TasaConversion");

            double limiteGlobal = _configuration.GetValue<double>(
                "EconomiaJuego:LimiteGlobalPPCoins");

            if (tasa <= 0 ||
                !double.IsFinite(limiteGlobal) ||
                limiteGlobal <= 0)
            {
                return (false, "La configuración de economía no es válida.");
            }

            if (puntosAConvertir % tasa != 0)
            {
                return (
                    false,
                    $"La conversión debe realizarse en múltiplos de {tasa} Points.");
            }

            decimal coinsDecimal = puntosAConvertir / tasa;
            double coinsARecibir = (double)coinsDecimal;

            if (!double.IsFinite(coinsARecibir) ||
                coinsARecibir <= 0)
            {
                return (false, "La cantidad de PP Coins no es válida.");
            }

            await using var transaccion =
                await _context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            try
            {
                var jugador = await _context.Jugadores
                    .FirstOrDefaultAsync(j => j.Id == idJugador);

                if (jugador == null)
                {
                    await transaccion.RollbackAsync();
                    return (false, "Jugador no encontrado.");
                }

                if (jugador.points < 0 ||
                    !double.IsFinite(jugador.ppCoins) ||
                    jugador.ppCoins < 0)
                {
                    await transaccion.RollbackAsync();

                    return (
                        false,
                        "Los saldos actuales del jugador no son válidos.");
                }

                if (jugador.points < puntosAConvertir)
                {
                    await transaccion.RollbackAsync();
                    return (false, "No tienes suficientes Points.");
                }

                double totalCoins = await _context.Jugadores
                    .SumAsync(j => j.ppCoins);

                if (!double.IsFinite(totalCoins) ||
                    totalCoins < 0 ||
                    totalCoins > limiteGlobal)
                {
                    await transaccion.RollbackAsync();

                    return (
                        false,
                        "El total global de PP Coins no respeta el límite permitido.");
                }

                double nuevoSaldo = jugador.ppCoins + coinsARecibir;

                if (!double.IsFinite(nuevoSaldo) ||
                    totalCoins + coinsARecibir > limiteGlobal)
                {
                    await transaccion.RollbackAsync();

                    return (
                        false,
                        $"Se alcanzó el límite global de {limiteGlobal:N0} PP Coins.");
                }

                jugador.points -= puntosAConvertir;
                jugador.ppCoins = nuevoSaldo;

                await _context.SaveChangesAsync();
                await transaccion.CommitAsync();

                return (
                    true,
                    $"Conversión exitosa. Recibiste {coinsARecibir:N0} PP Coins.");
            }
            catch
            {
                await transaccion.RollbackAsync();

                return (
                    false,
                    "Ocurrió un error al realizar la conversión.");
            }
        }
    }
}

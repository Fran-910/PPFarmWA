
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PPFarmWA.BD.Datos;
using PPFarmWA.BD.Datos.Entity;
using PPFarmWA.Shared.DTO;
using System.Data;

namespace PPFarmWA.Repositorio.Repositorios
{
    public class JugadorRepositorio : Repositorio<Jugador>, IJugadorRepositorio
    {
        private readonly IConfiguration _configuration;

        public JugadorRepositorio(
            AppDbContext context,
            IConfiguration configuration)
            : base(context)
        {
            _configuration = configuration;
        }

        // Modifica PP Coins respetando el límite global.
        public async Task<bool> ModificarCoinsAsync(
            int idJugador,
            double cantidad)
        {
            if (idJugador <= 0 ||
                double.IsNaN(cantidad) ||
                double.IsInfinity(cantidad))
            {
                return false;
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            try
            {
                var jugador = await _dbSet
                    .FirstOrDefaultAsync(j => j.Id == idJugador);

                if (jugador == null)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                double limiteGlobal = _configuration.GetValue<double>(
                    "EconomiaJuego:LimiteGlobalPPCoins");

                if (limiteGlobal <= 0)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                double totalCoins = await _context.Jugadores
                    .SumAsync(j => j.ppCoins);

                double nuevoSaldo = jugador.ppCoins + cantidad;
                double nuevoTotal = totalCoins + cantidad;

                if (nuevoSaldo < 0 ||
                    nuevoTotal < 0 ||
                    nuevoTotal > limiteGlobal)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                jugador.ppCoins = nuevoSaldo;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> ExisteEmailOusuarioAsync(
            string emailOusuario)
        {
            return await _dbSet.AnyAsync(
                j => j.email == emailOusuario ||
                     j.userName == emailOusuario);
        }

        public async Task<SesionDTO> RegistrarJugadorAsync(
            RegistroDTO dto)
        {
            var jugador = new Jugador
            {
                userName = dto.userName,
                email = dto.email,
                password = dto.password,
                ppCoins = 0,
                points = 0,
                level = 1,
                experiencia = 0,
                esTienda = false,
                esAdmin = false
            };

            _dbSet.Add(jugador);

            await _context.SaveChangesAsync();

            var recursoInicial = await _context.Recursos.FindAsync(1);

            Item itemInicial = new Item
            {
                cantidad = 1,
                durabilidad = recursoInicial?.durabilidad ?? 0,
                JugadorId = jugador.Id,
                RecursoId = 1
            };

            _context.Items.Add(itemInicial);

            await _context.SaveChangesAsync();

            jugador.idUltimaHerramienta = itemInicial.Id;

            await _context.SaveChangesAsync();

            var items = await _context.Items
                .Where(i => i.JugadorId == jugador.Id)
                .ToListAsync();

            return new SesionDTO
            {
                JugadorId = jugador.Id,
                userName = jugador.userName,
                email = jugador.email,
                ppCoins = jugador.ppCoins,
                points = jugador.points,
                level = jugador.level,
                experiencia = jugador.experiencia,
                IdUltimaHerramienta = jugador.idUltimaHerramienta,
                Items = items.Select(i => new ItemDTO
                {
                    Id = i.Id,
                    cantidad = i.cantidad,
                    idJugador = i.JugadorId,
                    idRecurso = i.RecursoId,
                    idVenta = i.VentaId,
                    durabilidad = i.durabilidad
                }).ToList()
            };
        }

        public async Task<SesionDTO> ObtenerSesionConItemsAsync(
            int idJugador)
        {
            var jugador = await _context.Jugadores
                .FirstOrDefaultAsync(j => j.Id == idJugador);

            if (jugador == null)
                return null;

            var items = await _context.Items
                .Where(i => i.JugadorId == idJugador)
                .ToListAsync();

            List<ItemDTO> item = new List<ItemDTO>();

            foreach (var i in items)
            {
                item.Add(new ItemDTO
                {
                    Id = i.Id,
                    cantidad = i.cantidad,
                    idJugador = i.JugadorId,
                    idRecurso = i.RecursoId,
                    idVenta = i.VentaId,
                    durabilidad = i.durabilidad
                });
            }

            return new SesionDTO
            {
                JugadorId = jugador.Id,
                userName = jugador.userName,
                email = jugador.email,
                ppCoins = jugador.ppCoins,
                points = jugador.points,
                level = jugador.level,
                experiencia = jugador.experiencia,
                IdUltimaHerramienta = jugador.idUltimaHerramienta,
                Items = item
            };
        }

        public async Task<SesionDTO>
            ObtenerSesionConItemsPorEmailYPasswordAsync(
                string email,
                string password)
        {
            var jugador = await _context.Jugadores
                .FirstOrDefaultAsync(j =>
                    j.email.ToLower() == email.ToLower() &&
                    j.password == password);

            if (jugador == null)
                return null;

            var items = await _context.Items
                .Where(i => i.JugadorId == jugador.Id)
                .ToListAsync();

            List<ItemDTO> item = new List<ItemDTO>();

            foreach (var i in items)
            {
                item.Add(new ItemDTO
                {
                    Id = i.Id,
                    cantidad = i.cantidad,
                    idJugador = i.JugadorId,
                    idRecurso = i.RecursoId,
                    idVenta = i.VentaId,
                    durabilidad = i.durabilidad
                });
            }

            return new SesionDTO
            {
                JugadorId = jugador.Id,
                userName = jugador.userName,
                email = jugador.email,
                ppCoins = jugador.ppCoins,
                points = jugador.points,
                level = jugador.level,
                experiencia = jugador.experiencia,
                IdUltimaHerramienta = jugador.idUltimaHerramienta,
                Items = item
            };
        }

        public async Task<SesionDTO> ObtenerSesionConItemsPorEmailAsync(
            string email)
        {
            var jugador = await _context.Jugadores
                .FirstOrDefaultAsync(j =>
                    j.email.ToLower() == email.ToLower());

            if (jugador == null)
                return null;

            var items = await _context.Items
                .Where(i => i.JugadorId == jugador.Id)
                .ToListAsync();

            List<ItemDTO> item = new List<ItemDTO>();

            foreach (var i in items)
            {
                item.Add(new ItemDTO
                {
                    Id = i.Id,
                    cantidad = i.cantidad,
                    idJugador = i.JugadorId,
                    idRecurso = i.RecursoId,
                    idVenta = i.VentaId,
                    durabilidad = i.durabilidad
                });
            }

            return new SesionDTO
            {
                JugadorId = jugador.Id,
                userName = jugador.userName,
                email = jugador.email,
                ppCoins = jugador.ppCoins,
                points = jugador.points,
                level = jugador.level,
                experiencia = jugador.experiencia,
                IdUltimaHerramienta = jugador.idUltimaHerramienta,
                Items = item
            };
        }

        public async Task<List<Item>> GetPlayerInv(int id)
        {
            return await _context.Items
                .Where(x => x.JugadorId == id)
                .ToListAsync();
        }

        public async Task<bool> UpdatePlayerInvAsync(
            List<ItemDTO> newTable,
            List<Item> bdTable)
        {
            bool changed = false;

            foreach (var item in newTable)
            {
                var matchedId = bdTable
                    .FirstOrDefault(x => x.Id == item.Id);

                if (matchedId == null)
                    continue;

                changed = true;
                matchedId.durabilidad = item.durabilidad;
            }

            foreach (var bdItem in bdTable)
            {
                if (bdItem.RecursoId != 1 &&
                    !newTable.Any(x => x.Id == bdItem.Id))
                {
                    _context.Items.Remove(bdItem);
                    changed = true;
                }
            }

            if (changed)
            {
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        // Acredita Points por cosecha exclusivamente desde el servidor.
        public async Task<(
            bool Exito,
            string Mensaje,
            int Points,
            int Recompensa)> AcreditarPointsPorCosechaAsync(
                int idJugador)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            try
            {
                var jugador = await _dbSet
                    .FirstOrDefaultAsync(j => j.Id == idJugador);

                if (jugador == null)
                {
                    await transaction.RollbackAsync();

                    return (
                        false,
                        "No se encontró el jugador.",
                        0,
                        0
                    );
                }

                int pointsPorNivel = _configuration.GetValue<int>(
                    "EconomiaJuego:PointsPorNivelCosecha",
                    2);

                if (jugador.level < 1 || pointsPorNivel <= 0)
                {
                    await transaction.RollbackAsync();

                    return (
                        false,
                        "La configuración de recompensa no es válida.",
                        jugador.points,
                        0
                    );
                }

                long recompensaCalculada =
                    (long)jugador.level * pointsPorNivel;

                if (recompensaCalculada > int.MaxValue ||
                    recompensaCalculada < 1 ||
                    jugador.points > int.MaxValue - recompensaCalculada)
                {
                    await transaction.RollbackAsync();

                    return (
                        false,
                        "La cantidad de Points supera el límite permitido.",
                        jugador.points,
                        0
                    );
                }

                int recompensa = (int)recompensaCalculada;

                jugador.points += recompensa;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (
                    true,
                    $"Se acreditaron {recompensa} Points correctamente.",
                    jugador.points,
                    recompensa
                );
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

using PPFarmWA.BD.Datos;
using PPFarmWA.BD.Datos.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Repositorio.Repositorios
{
    public class JugadorRepositorio : Repositorio<Jugador>, IJugadorRepositorio
    {
        public JugadorRepositorio(AppDbContext context)
            : base(context)
        {
        }

        public async Task<bool> ModificarCoinsAsync(int idJugador, double cantidad)
        {
            var jugador = await _dbSet.FindAsync(idJugador);
            if (jugador == null)
                return false;

            jugador.ppCoins += cantidad;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

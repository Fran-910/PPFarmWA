using PPFarmWA.BD.Datos;
using PPFarmWA.BD.Datos.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Repositorio.Repositorios
{
    public class ItemRepositorio : Repositorio<Item>, IItemRepositorio
    {
        public ItemRepositorio(AppDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Item>> GetInventarioJugadorAsync(int idJugador)
        {
            return await _dbSet
                .Where(i => i.idJugador == idJugador)
                .ToListAsync();
        }
    }
}

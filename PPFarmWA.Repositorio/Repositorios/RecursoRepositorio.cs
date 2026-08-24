using PPFarmWA.BD.Datos;
using PPFarmWA.BD.Datos.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Repositorio.Repositorios
{
    public class RecursoRepositorio : Repositorio<Recurso>, IRecursoRepositorio
    {
        public RecursoRepositorio(AppDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Recurso>> GetRecursosTiendaAsync()
        {
            return await _dbSet
                .Where(r => r.deTienda)
                .ToListAsync();
        }
    }
}

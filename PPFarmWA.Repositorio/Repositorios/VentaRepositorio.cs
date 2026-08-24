using PPFarmWA.BD.Datos;
using PPFarmWA.BD.Datos.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Repositorio.Repositorios
{
    public class VentaRepositorio : Repositorio<Venta>, IVentaRepositorio
    {
        public VentaRepositorio(AppDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Venta>> GetVentasJugadorAsync(int idJugador)
        {
            return await _dbSet
                .Where(v =>
                    v.idJugadorVendedor == idJugador ||
                    v.idJugadorComprador == idJugador)
                .ToListAsync();
        }
    }
}

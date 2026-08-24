using PPFarmWA.BD.Datos.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Repositorio.Repositorios
{
    public interface IItemRepositorio : IRepositorio<Item>
    {
        Task<IEnumerable<Item>> GetInventarioJugadorAsync(int idJugador);
    }
}

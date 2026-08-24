using PPFarmWA.BD.Datos.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Repositorio.Repositorios
{
    public interface IVentaRepositorio : IRepositorio<Venta>
    {
        Task<IEnumerable<Venta>> GetVentasJugadorAsync(int idJugador);
    }
}

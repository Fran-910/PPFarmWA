using PPFarmWA.BD.Datos.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Repositorio.Repositorios
{
    public interface IRecursoRepositorio : IRepositorio<Recurso>
    {
        Task<IEnumerable<Recurso>> GetRecursosTiendaAsync();
    }
}

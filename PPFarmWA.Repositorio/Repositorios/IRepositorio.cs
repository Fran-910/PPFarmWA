using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Repositorio.Repositorios
{
    public interface IRepositorio<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}

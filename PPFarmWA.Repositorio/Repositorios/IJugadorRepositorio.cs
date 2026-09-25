using PPFarmWA.BD.Datos.Entity;
using PPFarmWA.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.Repositorio.Repositorios
{
    public interface IJugadorRepositorio : IRepositorio<Jugador>
    {
        Task<bool> ModificarCoinsAsync(int idJugador, double cantidad);

        Task<bool> ExisteEmailOusuarioAsync(string emailOusuario);

        Task<SesionDTO> RegistrarJugadorAsync(RegistroDTO dto);
    }
}

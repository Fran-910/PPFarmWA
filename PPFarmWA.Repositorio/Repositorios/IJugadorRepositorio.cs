
using PPFarmWA.BD.Datos.Entity;
using PPFarmWA.Shared.DTO;

namespace PPFarmWA.Repositorio.Repositorios
{
    public interface IJugadorRepositorio : IRepositorio<Jugador>
    {
        Task<bool> ModificarCoinsAsync(int idJugador, double cantidad);

        Task<bool> ExisteEmailOusuarioAsync(string emailOusuario);

        Task<SesionDTO> RegistrarJugadorAsync(RegistroDTO dto);

        Task<SesionDTO> ObtenerSesionConItemsAsync(int idJugador);

        Task<SesionDTO> ObtenerSesionConItemsPorEmailAsync(string email);

        Task<SesionDTO> ObtenerSesionConItemsPorEmailYPasswordAsync(
            string email,
            string password);

        Task<List<Item>> GetPlayerInv(int id);

        Task<bool> UpdatePlayerInvAsync(
            List<ItemDTO> newTable,
            List<Item> bdTable);

        // Acredita Points por cosecha desde el servidor.
        Task<(bool Exito, string Mensaje, int Points, int Recompensa)>
            AcreditarPointsPorCosechaAsync(int idJugador);
    }
}

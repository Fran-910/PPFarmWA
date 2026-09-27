using Microsoft.EntityFrameworkCore;
using PPFarmWA.BD.Datos;
using PPFarmWA.BD.Datos.Entity;
using PPFarmWA.Shared.DTO;
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
       public async Task<bool> ExisteEmailOusuarioAsync(string emailOusuario)
        {
            return await _dbSet.AnyAsync(j => j.email == emailOusuario || j.userName == emailOusuario);
        }
        public async Task<SesionDTO> RegistrarJugadorAsync(RegistroDTO dto)
        {
            var jugador = new Jugador
            {
                userName = dto.userName,
                email = dto.email,
                password = dto.password,
                ppCoins = 0,
                points = 0,
                level = 1,
                experiencia = 0,
                esTienda = false,
                esAdmin = false
            };
            _dbSet.Add(jugador);
            await _context.SaveChangesAsync();
            return new SesionDTO
            {
                userName = jugador.userName,
                email = jugador.email,
                ppCoins = jugador.ppCoins,
                points = jugador.points,
                level = jugador.level,
                experiencia = jugador.experiencia
            };
        }
        public async Task<SesionDTO> ObtenerSesionConItemsAsync(int idJugador)
        {
            var jugador = await _context.Jugadores.Where(j => j.Id == idJugador)
                .FirstOrDefaultAsync();
            var items = await _context.Items.Where(i => i.JugadorId == idJugador).ToListAsync();
            List<ItemDTO> item = new List<ItemDTO>();
            foreach (var i in items)
            {
                item.Add(new ItemDTO
                {
                    Id = i.Id,
                    cantidad = i.cantidad,
                    idJugador = i.JugadorId,
                    idRecurso = i.RecursoId,
                    idVenta = i.VentaId
                });
            }

            if (jugador == null)
                return null;

            return new SesionDTO
            {   
                userName = jugador.userName,
                email = jugador.email,
                ppCoins = jugador.ppCoins,
                points = jugador.points,
                level = jugador.level,
                experiencia = jugador.experiencia,
                IdUltimaHerramienta = jugador.idUltimaHerramienta,
                Items = item
            };
        }
    }
}

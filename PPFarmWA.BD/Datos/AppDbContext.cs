using Microsoft.EntityFrameworkCore;
using PPFarmWA.BD.Datos.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPFarmWA.BD.Datos
{
    public class AppDbContext : DbContext
    {
        public DbSet<Jugador> Jugadores { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Recurso> Recursos { get; set; }
        public DbSet<Item> Items { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}

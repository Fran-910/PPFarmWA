using PPFarmWA.BD.Datos;
using PPFarmWA.BD.Datos.Entity;

namespace PPFarmWA.Server
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            var recursosIniciales = new List<Recurso>()
            {
                new Recurso()
                {
                    nombre = "Pala",
                    descripcion = "Tu primera herramienta para gestionar tus cultivos.",
                    eficiencia = 1,
                    durabilidad = -1,
                    valor = 0,
                    deTienda = false,
                    idRareza = Shared.Enum.RarezaEnum.Comun
                },
                new Recurso()
                    {
                        nombre = "Pico",
                        descripcion = "",
                        eficiencia = 5,
                        durabilidad = 100,
                        valor = 5,
                        deTienda = true,
                        idRareza = Shared.Enum.RarezaEnum.Comun
                    },
                    new Recurso()
                    {
                        nombre = "Pala de Oro",
                        descripcion = "",
                        eficiencia = 10,
                        durabilidad = 150,
                        valor = 10,
                        deTienda = true,
                        idRareza = Shared.Enum.RarezaEnum.Comun
                    }
            };

            foreach (var recurso in recursosIniciales)
            {
                if (!db.Recursos.Any(r => r.nombre == recurso.nombre))
                {
                    db.Recursos.Add(recurso);
                }
            }

            db.SaveChanges();
        }
    }
}
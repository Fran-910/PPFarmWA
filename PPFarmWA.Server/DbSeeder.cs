using PPFarmWA.BD.Datos;
using PPFarmWA.BD.Datos.Entity;

namespace PPFarmWA.Server
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if(!db.Recursos.Any())
            {
                var recursosIniciales = new List<Recurso>()
                {
                    new Recurso(){nombre = "Pala", descripcion = "Tu primera herramienta para gestionar tus cultivos.", eficiencia = 1, durabilidad = -1, valor = 0, deTienda = false, idRareza = Shared.Enum.RarezaEnum.Comun}
                };

                db.Recursos.AddRange(recursosIniciales);

                db.SaveChanges();
            }
        }
    }
}

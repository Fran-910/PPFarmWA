using PPFarmWA.Shared.DTO;

namespace PPFramWA.Client.Services
{
    public class CatalogoRecursos
    {
        private readonly RecursoServicio __recursoServicio;
        public List<RecursoDTO> recursos { get; set; }
        public bool cargado { get; set; } = false;
        public CatalogoRecursos(RecursoServicio recursoServicio)
        {
            __recursoServicio = recursoServicio;
        }

        public async Task<List<RecursoDTO>> ObtenerRecursosAsync()
        {
            if (cargado) { return recursos; }
            recursos = await __recursoServicio.ObtenerTodos();
            cargado = true;
            return recursos;
        }
    }
}

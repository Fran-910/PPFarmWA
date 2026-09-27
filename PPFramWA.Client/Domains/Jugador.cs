using PPFarmWA.Shared.DTO;

namespace PPFramWA.Client.Domains
{
    public class Jugador
    {
        public int Id { get; set; }
        public int level { get; set; }
        public int experiencia { get; set; }
        public int ppCoins { get; set; }
        public int points { get; set; }
        private int _idUltimaHerramienta;
        public int idUltimaHerramienta 
        { 
            get => _idUltimaHerramienta;
            set
            {
                if(_idUltimaHerramienta != value)
                {
                    _idUltimaHerramienta = value;
                    OnCambioUltimaHerramienta?.Invoke();
                }
            }
        }
        public int idUltimoRecurso { get; set; } = 1;
        public event Action? OnCambioUltimaHerramienta;
        public int experienciaParaSubir { get; set; }

        public event Action? OnSubioNivel;

        // RECURSODTO

        public RecursoDTO recurso { get; set; }
        public Jugador() // Posiblemente acá venga un DTO que llene los datos de la clase
        {
            calcularExpNecesaria();
        }

        public void CambioUltimaHerramienta(int idHerramienta)
        {
            idUltimaHerramienta = idHerramienta;
            // Select de los items del inventario del jugador en memoria para obtener la herramienta con el id correspondiente
            // idUltimoRecurso = herramienta.idRecurso; con esa prop se puede obtener el recurso en los componentes
            OnCambioUltimaHerramienta?.Invoke();
        }
        public void calcularExpNecesaria()
        {
            experienciaParaSubir = 50 + (int)(MathF.Pow(Math.Max(level - 1, 0), 2.2f) * 15);
        }
        public void SubirDeNivel()
        {
            if (experiencia > experienciaParaSubir)
            {
                level++;
                experiencia = 0;
                calcularExpNecesaria();
                OnSubioNivel?.Invoke();
            }
        }
    }
}

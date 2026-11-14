using PPFarmWA.Shared.DTO;

namespace PPFramWA.Client.Domains
{
    public class Jugador
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public int level { get; set; }
        public int experiencia { get; set; }
        public double ppCoins { get; set; }
        public int points { get; set; }
        public List<ItemDTO> items { get; set; } = new List<ItemDTO>();
        public ItemDTO currentItem { get; set; }
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
        public event Action? OnCambioUltimaHerramienta;
        public int experienciaParaSubir { get; set; }
        public event Action? OnSubioNivel;
        public event Action<ItemDTO>? OnItemDestroy;
        public event Action? OnItemDestroySave;

        // RECURSODTO

        public RecursoDTO recurso { get; set; }
        public Jugador(SesionDTO dto) // Posiblemente acá venga un DTO que llene los datos de la clase
        {
            Id = dto.JugadorId;
            userName = dto.userName;
            email = dto.email;
            level = dto.level;
            experiencia = dto.experiencia;
            ppCoins = dto.ppCoins;
            points = dto.points;
            idUltimaHerramienta = dto.IdUltimaHerramienta;
            items = dto.Items;
            calcularExpNecesaria();

            currentItem = items.Where(x => x.Id == idUltimaHerramienta).FirstOrDefault();
        }

        public void CambioUltimaHerramienta(int idItem)
        {
            // El currentItem se actualiza ANTES de disparar el evento:
            // si no, SetUpTool corre con la herramienta anterior (o con null).
            currentItem = items.FirstOrDefault(x => x.Id == idItem);

            if (_idUltimaHerramienta == idItem)
                OnCambioUltimaHerramienta?.Invoke();
            else
                idUltimaHerramienta = idItem; // el setter dispara el evento una sola vez
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
        public void RefrescarItems(List<ItemDTO> items)
        {
            this.items = items;
            currentItem = items.Where(x => x.Id == idUltimaHerramienta).FirstOrDefault();
        }

        public void ItemDestroy(ItemDTO item)
        {
            items.Remove(item);
            if (currentItem != null && currentItem.Id == item.Id)
            {
                currentItem = null;
                idUltimaHerramienta = 0;
            }

            OnItemDestroySave?.Invoke();
            OnItemDestroy?.Invoke(item);
        }
    }
}

namespace PPFramWA.Client.Domains
{
    public class Jugador
    {
        public int Id { get; set; }
        public int level { get; set; }
        public int experiencia { get; set; }
        public int ppCoins { get; set; }
        public int points { get; set; }
        public int experienciaParaSubir { get; set; }

        public event Action? OnSubioNivel;
        public Jugador() // Posiblemente acá venga un DTO que llene los datos de la clase
        {
            calcularExpNecesaria();
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

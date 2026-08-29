namespace PPFramWA.Client.Domains
{
    public class Jugador
    {
        public int Id { get; set; }
        public int level { get; set; }
        public int experiencia { get; set; }
        public int ppCoins { get; set; }
        public int points { get; set; }

        public Jugador() // Posiblemente acá venga un DTO que llene los datos de la clase
        {

        }

        public void SubirDeNivel()
        {

        }
    }
}

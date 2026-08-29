using System.Runtime.CompilerServices;

namespace PPFramWA.Client.Domains
{
    public class Cultivo
    {
        public int vida { get; set; }
        public int xp { get; set; }
        public int points { get; set; }

        public Jugador __jugador;

        //Valores base
        public float VIDA_BASE { get; set; } = 10f;
        public float ESCALA { get; set; } = 5f;

        public Cultivo(Jugador jugador)
        {
            __jugador = jugador;

            int vidaCalculada = (int)MathF.Round(VIDA_BASE + MathF.Pow(__jugador.level - 1, 2) * ESCALA);

            vida = vidaCalculada;
        }

        //Logica de hit para reducir la vida de los cultivos y ejecutar cosechado
        public void Golpeado(int damage)
        {



        }

        public void Cosechado()
        {
            var xpGain = __jugador.level * 5;
            var pointsGain = __jugador.level * 2;

            xp = xpGain;
            points = pointsGain;

            __jugador.experiencia = __jugador.experiencia + xpGain;
            __jugador.points = __jugador.points + pointsGain;
        }

        public async Task CooldownCultivo()
        {
            double cooldown = 10 / (1 + (__jugador.level - 1) * 0.1);

            await Task.Delay(TimeSpan.FromSeconds(cooldown));
        }
    }
}

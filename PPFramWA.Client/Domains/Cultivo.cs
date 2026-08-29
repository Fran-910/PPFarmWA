using PPFramWA.Client.Services;
using System.Runtime.CompilerServices;

namespace PPFramWA.Client.Domains
{
    public class Cultivo
    {
        public int vida { get; set; }
        public int xp { get; set; }
        public int points { get; set; }

        public JugadorState jugadorStateCultivo;

        //Valores base
        public float VIDA_BASE { get; set; } = 10f;
        public float ESCALA { get; set; } = 5f;

        public Cultivo(JugadorState jugadorState)
        {
            jugadorStateCultivo = jugadorState;
            int vidaCalculada = (int)MathF.Round(VIDA_BASE + MathF.Pow(jugadorStateCultivo.__jugador!.level - 1, 2) * ESCALA);

            vida = vidaCalculada;
        }

        //Logica de hit para reducir la vida de los cultivos y ejecutar cosechado
        public void Golpeado(int damage)
        {



        }

        public void Cosechado()
        {
            var xpGain = jugadorStateCultivo.__jugador!.level * 5;
            var pointsGain = jugadorStateCultivo.__jugador!.level * 2;

            xp = xpGain;
            points = pointsGain;

            jugadorStateCultivo.__jugador!.experiencia = jugadorStateCultivo.__jugador!.experiencia + xpGain;
            jugadorStateCultivo.__jugador!.points = jugadorStateCultivo.__jugador!.points + pointsGain;

            jugadorStateCultivo.NotificarCambios();
            jugadorStateCultivo.__jugador!.SubirDeNivel();
        }

        public async Task CooldownCultivo()
        {
            double cooldown = 10 / (1 + (jugadorStateCultivo.__jugador!.level - 1) * 0.1);

            await Task.Delay(TimeSpan.FromSeconds(cooldown));
        }
    }
}

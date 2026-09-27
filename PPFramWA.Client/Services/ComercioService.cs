using System.Threading.Tasks;

namespace PPFramWA.Client.Services 
{
    public class ComercioServicio
    {
        // Metodo "Falso"
        public async Task<RespuestaFalsa> ConvertirPoints(int puntosConvertidos)
        {
            // Delay de 1 segundo para simular la latencia de una llamada a un servidor real
            await Task.Delay(1000);

            // Devolver una respuesta simulada
            return new RespuestaFalsa
            {
                Exito = true,
                Mensaje = $"¡Conversión exitosa! El servidor aprobó tus {puntosConvertidos} Points."
            };
        }
    }

    // Una caja de datos para la respuesta simulada
    public class RespuestaFalsa
    {
        public bool Exito { get; set; }
        public string? Mensaje { get; set; }
    }
}
namespace Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.Respuestas
{
    public class RespuestasUpdateCommand
    {
        public int CedulaEvaluacionId { get; set; }
        public int Pregunta { get; set; }
        public bool Respuesta { get; set; }
        public string Detalles { get; set; }
        
        public string UsuarioId { get; set; }

    }
}

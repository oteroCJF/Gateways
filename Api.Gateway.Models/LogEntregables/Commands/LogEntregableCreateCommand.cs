namespace Api.Gateway.Models.LogEntregables.Commands
{
    public class LogEntregableCreateCommand
    {
        public int CedulaEvaluacionId { get; set; }
        public int EstatusId { get; set; }
        public int EntregableId { get; set; }
        public string UsuarioId { get; set; }
        public string Observaciones { get; set; }
    }
}

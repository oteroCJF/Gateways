using System;

namespace Api.Gateway.Models.LogSolicitudes.Commands
{
    public class LogSolicitudCreateCommand
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; }
        public int EstatusId { get; set; }
        public string UsuarioId { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}

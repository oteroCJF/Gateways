using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.LogsEntregables.Commands
{
    public class LogSBEntregableCreateCommand
    {
        public int SolicitudId { get; set; }
        public int EstatusId { get; set; }
        public int EntregableId { get; set; }
        public string UsuarioId { get; set; }
        public string Observaciones { get; set; }
    }
}

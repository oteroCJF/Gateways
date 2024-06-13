using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.LogsCedulas.Commands
{
    public class LogCedulaCreateCommand
    {
        public int CedulaEvaluacionId { get; set; }
        public int EstatusId { get; set; }
        public string UsuarioId { get; set; }
        public string Observaciones { get; set; }
    }
}

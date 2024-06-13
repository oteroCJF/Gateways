using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Create
{
    public class EntregableCreateCommand
    {
        public int Id { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public string? UsuarioId { get; set; }
        public int EstatusId { get; set; }
        public int EntregableId { get; set; }
        public string Entregable { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}

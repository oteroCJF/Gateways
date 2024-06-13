using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas
{
    public class EEntregableUpdateCommand
    {
        public int Id { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int EntregableId { get; set; }
        public string UsuarioId { get; set; }
        public int EstatusId { get; set; }
        public string Observaciones { get; set; }
        public string Estatus { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime FechaEliminacion { get; set; }

    }
}

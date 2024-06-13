using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas
{
    public class ESREntregableUpdateCommand
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int EstatusId { get; set; }
        public bool Aprobada { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaEliminacion { get; set; }
    }
}

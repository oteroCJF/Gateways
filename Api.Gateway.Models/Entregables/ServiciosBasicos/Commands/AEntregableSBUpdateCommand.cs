using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Entregables.ServiciosBasicos.Commands
{
    public class AEntregableSBUpdateCommand
    {
        public int Id { get; set; }
        public int EntregableId { get; set; }
        public int SolicitudId { get; set; }
        public string UsuarioId { get; set; }
        public int EstatusId { get; set; }
        public string Estatus { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.SolicitudesPago.Commands
{
    public class SolicitudPagoUpdateCommand
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int EstatusId { get; set; }
        public int EstatusEntregableId { get; set; }
        public Nullable<DateTime> FechaActualizacion { get; set; }
    }
}

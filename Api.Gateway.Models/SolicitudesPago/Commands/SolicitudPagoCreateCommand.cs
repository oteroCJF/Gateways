using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.SolicitudesPago.Commands
{
    public class SolicitudPagoCreateCommand
    {
        public int ServicioId { get; set; }
        public string UsuarioId { get; set; }
        public int InmuebleId { get; set; }
        public int Anio { get; set; }
        public int MesId { get; set; }
        public int EstatusId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
    }
}

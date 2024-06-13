using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.BMuebles.Solicitudes.Commands
{
    public class SolicitudCreateCommand
    {
        public int Id { get; set; }
        public int TipoId { get; set; }
        public string UsuarioId { get; set; }
        public int ContratoId { get; set; }
        public int InmuebleId { get; set; }
        public int PartidaId { get; set; }
        public int EstatusId { get; set; }
        public int Anio { get; set; }
        public int MesId { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaServicio { get; set; }
        public string HoraServicio { get; set; }
        public int OrigenId { get; set; }
        public int DestinoId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }

        public DetalleSolicitudCreateCommand Detalle { get; set; } = new DetalleSolicitudCreateCommand();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.BMuebles.Solicitudes.Commands
{
    public class DetalleSolicitudCreateCommand
    {
        public int SolicitudId { get; set; }
        public string Concepto { get; set; }
        public int Unidades { get; set; }
        public int Estibadores { get; set; }
        public string UEntregaId { get; set; }
        public string URecibeId { get; set; }
        public string Telefono { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}

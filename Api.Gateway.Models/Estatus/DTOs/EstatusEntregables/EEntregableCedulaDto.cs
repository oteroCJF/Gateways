using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Estatus.DTOs.EstatusEntregables
{
    public class EEntregableCedulaDto
    {
        public int EntregableId { get; set; }
        public int EEstatusId { get; set; }
        public int EstatusId { get; set; }
        public string Flujo { get; set; }
        public string Observaciones { get; set; }
    }
}

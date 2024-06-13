using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Estatus.DTOs.EstatusEntregables
{
    public class FlujoEntregableDto
    {
        public int ServicioId { get; set; }
        public int EntregableId { get; set; }
        public int EstatusId { get; set; }
        public int ESucesivoId { get; set; }
        public string Flujo { get; set; }
        public bool Editable { get; set; }
        public bool Autorizar { get; set; }
        public bool Rechazar { get; set; }
        public bool Validar { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Estatus.DTOs.EstatusCedulas
{
    public class FlujoServicioDto
    {
        public int ServicioId { get; set; }
        public int EstatusId { get; set; }
        public int ESucesivoId { get; set; }
        public int EFacturaId { get; set; }
        public string? Flujo { get; set; }
        public string? Boton { get; set; }

        public EstatusDto ESucesivo { get; set; } = new EstatusDto();
    }
}

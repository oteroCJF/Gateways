using Api.Gateway.Models.Estatus.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Flujos.DTOs
{
    public class FlujoDto
    {
        public int EstatusCedulaId { get; set; }
        public int EstatusId { get; set; }
        public string Flujo { get; set; }
        public EstatusDto Estatus { get; set; } = new EstatusDto();
    }
}

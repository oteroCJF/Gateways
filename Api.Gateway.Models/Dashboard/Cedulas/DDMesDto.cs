using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Dashboard.Cedulas
{
    public class DDMesDto
    {
        public CTServicioDto Servicio { get; set; } = new CTServicioDto();
        public List<CedulaDto> Cedulas { get; set; } = new List<CedulaDto>();
    }
}

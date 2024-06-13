using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Models.Dashboard.Cedulas;
using Api.Gateway.Models.Dashboard.Financieros;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Dashboard.Cedulas
{
    public class DCedulaDto
    {
        public CTServicioDto Servicio { get; set; } = new CTServicioDto();
        public List<CedulaDto> Cedulas { get; set; } = new List<CedulaDto>();
    }
}

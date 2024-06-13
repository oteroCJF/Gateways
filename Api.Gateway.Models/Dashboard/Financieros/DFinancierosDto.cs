using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Dashboard.Financieros
{
    public class DFinancierosDto
    {
        public CTServicioDto Servicio { get; set; } = new CTServicioDto();
        public FacturaDto Facturas { get; set; } = new FacturaDto();
        public FacturaDto NC { get; set; } = new FacturaDto();
    }
}

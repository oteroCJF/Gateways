using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Models.Catalogos.DTOs.ServiciosContratos;
using Api.Gateway.Models.Parametrizacion.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Contratos.DTOs
{
    public class ServicioContratoDto
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public int ServicioId { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public decimal PorcentajeImpuesto { get; set; }
        public virtual CTServicioContratoDto Servicio { get; set; } = new CTServicioContratoDto();
    }
}

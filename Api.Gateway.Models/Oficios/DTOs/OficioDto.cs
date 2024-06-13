using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using Api.Gateway.Models.Estatus.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Oficios.DTOs
{
    public class OficioDto
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public int? EstatusId { get; set; }
        public EstatusDto Estatus { get; set; } = new EstatusDto();
        public int? Anio { get; set; }
        public string NumeroOficio { get; set; }
        public DateTime? FechaTramitado { get; set; }
        public DateTime? FechaPagado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }

        public List<FCFDIDto> CFDIs { get; set; } = new List<FCFDIDto>();
    }
}

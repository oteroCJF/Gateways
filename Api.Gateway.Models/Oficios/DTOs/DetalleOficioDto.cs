using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Oficios.DTOs
{
    public class DetalleOficioDto
    {
        public int ServicioId { get; set; }
        public int OficioId { get; set; }
        public int FacturaId { get; set; }
        public int CedulaId { get; set; }
    }
}

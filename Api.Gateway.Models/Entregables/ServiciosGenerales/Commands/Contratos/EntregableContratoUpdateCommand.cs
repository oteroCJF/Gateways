using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Contratos
{
    public class EntregableContratoUpdateCommand
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        public int ContratoId { get; set; }
        public int ConvenioId { get; set; }
        public int EntregableId { get; set; }
        public IFormFile Archivo { get; set; }
        public DateTime? FechaProgramada { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public DateTime? InicioVigencia { get; set; }
        public DateTime? FinVigencia { get; set; }
        public decimal? MontoGarantia { get; set; }
        public bool? Penalizable { get; set; }
        public decimal? MontoPenalizacion { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        public string Contrato { get; set; }
        public string Convenio { get; set; } = string.Empty;
        public string TipoEntregable { get; set; }
    }
}

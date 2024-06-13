using System;
using System.Collections.Generic;

namespace Api.Gateway.Models.CFDIs.ServiciosBasicos.DTOs
{
    public class CFDISBDto
    {
        public int Id { get; set; }
        public int? SolicitudId { get; set; }
        public int? EstatusId { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string RFC { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Serie { get; set; } = string.Empty;
        public long? Folio { get; set; }
        public string UsoCFDI { get; set; } = string.Empty;
        public string UUID { get; set; } = string.Empty;
        public DateTime? FechaTimbrado { get; set; }
        public decimal? IVA { get; set; }
        public decimal? RetencionIVA { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Total { get; set; }
        public string Estatus { get; set; } = string.Empty;
        public string ArchivoXML { get; set; } = string.Empty;
        public string ArchivoPDF { get; set; } = string.Empty;

        public virtual List<ConceptoCFDISBDto> Conceptos { get; set; } = new List<ConceptoCFDISBDto>();
        public virtual List<ASGeneralesDto> Axa { get; set; } = new List<ASGeneralesDto>();
    }
}

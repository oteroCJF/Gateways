using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Inmuebles.DTOs.Inmuebles;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs
{
    public class FCFDIDto
    {
        public int Id { get; set; }
        public int CedulaId { get; set; }
        public int RepositorioId { get; set; }
        public int InmuebleId { get; set; }
        public int EstatusId { get; set; }
        public virtual EstatusDto Estatus { get; set; } = new EstatusDto();
        public int EFacturaId{ get; set; }
        public virtual EstatusDto EFactura { get; set; } = new EstatusDto();
        public string Inmueble { get; set; }
        public string UsuarioId { get; set; }
        public string Usuario { get; set; }
        public string Tipo { get; set; }
        public string Facturacion { get; set; }
        public string RFC { get; set; }
        public string Nombre { get; set; }
        public string Serie { get; set; }
        public long? FolioFactura { get; set; }
        public string FolioCedula { get; set; }
        public decimal Calificacion { get; set; }
        public string UsoCFDI { get; set; }
        public string UUID { get; set; }
        public string? UUIDRelacionado { get; set; }
        public DateTime FechaTimbrado { get; set; }
        public decimal? IVA { get; set; }
        public decimal? RetencionIVA { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Total { get; set; }
        public string ArchivoXML { get; set; }
        public string ArchivoPDF { get; set; }
        public DateTime FechaCreacion { get; set; }
        public virtual List<ConceptoCFDIDto> ConceptosFactura { get; set; } = new List<ConceptoCFDIDto>();

    }
}

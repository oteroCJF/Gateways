using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Inmuebles.DTOs.Inmuebles;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.CFDIs.ServiciosGenerales.DTOs
{
    public class CFDIDto
    {
        public int Id { get; set; }
        public int RepositorioId { get; set; }
        public int InmuebleId { get; set; }
        public int EstatusId { get; set; }
        public virtual InmuebleDto Inmueble { get; set; } = new InmuebleDto();
        public string UsuarioId { get; set; }
        public virtual UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public string Tipo { get; set; }
        public string Facturacion { get; set; }
        public string RFC { get; set; }
        public string Nombre { get; set; }
        public string Serie { get; set; }
        public long? Folio { get; set; }
        public string UsoCFDI { get; set; }
        public string UUID { get; set; }
        public string UUIDRelacionado { get; set; }
        public DateTime FechaTimbrado { get; set; }
        public decimal? IVA { get; set; }
        public decimal? RetencionIVA { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Total { get; set; }
        public string ArchivoXML { get; set; }
        public string ArchivoPDF { get; set; }
        public DateTime FechaCreacion { get; set; }
        public virtual IEnumerable<ConceptoCFDIDto> ConceptosFactura { get; set; } = new List<ConceptoCFDIDto>();
        public virtual EstatusDto Estatus { get; set; } = new EstatusDto();
        public virtual CedulaEvaluacionDto Cedula { get; set; } = new CedulaEvaluacionDto();
    }
}

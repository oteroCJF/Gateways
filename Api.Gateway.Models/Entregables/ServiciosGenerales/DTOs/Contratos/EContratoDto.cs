using Api.Gateway.Models.Catalogos.DTOs.Entregables;
using Api.Gateway.Models.Parametrizacion.DTOs;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Contratos
{
    public class EContratoDto
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        public int ContratoId { get; set; }
        public int? ConvenioId { get; set; }
        public int EntregableId { get; set; }
        public string Archivo { get; set; } = string.Empty;
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
        public DateTime? FechaEliminacion { get; set; }

        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public CTEntregableDto TipoEntregable { get; set; } = new CTEntregableDto();
    }
}

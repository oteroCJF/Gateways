using Api.Gateway.Models.Catalogos.DTOs.Incidencias;
using Api.Gateway.Models.Catalogos.DTOs.Indemnizaciones;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Transporte.DTOs
{
    public class TIncidenciaDto
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int IncidenciaId { get; set; }
        public int Pregunta { get; set; }
        public DateTime? FechaIncidencia { get; set; }
        public string? HoraIncidencia { get; set; }
        public string Observaciones { get; set; }
        public bool Penalizable { get; set; }
        public decimal MontoPenalizacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }

        public TConfiguracionIncidenciaDto ciTransporte { get; set; } = new TConfiguracionIncidenciaDto();
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public CTIncidenciaDto Incidencia { get; set; } = new CTIncidenciaDto();
    }
}

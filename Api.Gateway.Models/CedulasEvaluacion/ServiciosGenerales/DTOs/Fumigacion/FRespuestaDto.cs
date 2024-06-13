using System;
using System.Collections.Generic;
using Api.Gateway.Models.Cuestionarios.DTOs;
using Api.Gateway.Models.Incidencias.Fumigacion.DTOs;

namespace Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Fumigacion
{
    public class FRespuestaDto
    {
        public int CedulaEvaluacionId { get; set; }
        public int Pregunta { get; set; }
        public bool? Respuesta { get; set; }
        public string Detalles { get; set; } = string.Empty;
        public bool? Penalizable { get; set; } = false;
        public decimal? MontoPenalizacion { get; set; } = 0;
        public DateTime? FechaCreacion { get; set; } = Convert.ToDateTime("01/01/1990");
        public DateTime? FechaActualizacion { get; set; } = Convert.ToDateTime("01/01/1990");
        public DateTime? FechaEliminacion { get; set; } = Convert.ToDateTime("01/01/1990");
        public CuestionarioDto cuestionario { get; set; } = null;

        public FConfiguracionIncidenciaDto ciFumigacion { get; set; } = new FConfiguracionIncidenciaDto();

        public IEnumerable<FIncidenciaDto> iFumigacion { get; set; } = new List<FIncidenciaDto>();
    }
}

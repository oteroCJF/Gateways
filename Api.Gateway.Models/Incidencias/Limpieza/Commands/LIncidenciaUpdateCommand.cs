using Api.Gateway.Models.Contratos.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Limpieza.Commands
{
    public class LIncidenciaUpdateCommand
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int IncidenciaId { get; set; }
        public int DIncidenciaId { get; set; }
        public int MesId { get; set; }
        public int Pregunta { get; set; }
        public DateTime? FechaIncidencia { get; set; }
        public int? Inasistencias { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public virtual List<ServicioContratoDto> Penalizacion { get; set; } = new List<ServicioContratoDto>();

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Fumigacion.Commands
{
    public class FIncidenciaUpdateCommand
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int IncidenciaId { get; set; }
        public int DIncidenciaId { get; set; }
        public int Pregunta { get; set; }
        public bool Penalizable { get; set; }
        public int MesId { get; set; }
        public decimal MontoPenalizacion { get; set; }
        public DateTime? FechaProgramada { get; set; }
        public DateTime? FechaRealizada{ get; set; }
        public DateTime? FechaReaparicion { get; set; }
        public TimeSpan HoraProgramada { get; set; }
        public TimeSpan HoraRealizada { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public string? Observaciones { get; set; }
    }
}

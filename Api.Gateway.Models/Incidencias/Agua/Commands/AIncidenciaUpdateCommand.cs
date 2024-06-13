using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Agua.Commands
{
    public class AIncidenciaUpdateCommand
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int IncidenciaId { get; set; }
        public int TipoId { get; set; }
        public int Pregunta { get; set; }
        public DateTime FechaProgramada { get; set; }
        public DateTime FechaEntrega { get; set; }
        public TimeSpan HoraProgramada { get; set; }
        public TimeSpan HoraRealizada { get; set; }
        public int Cantidad { get; set; }
        public string? Observaciones { get; set; }
        public bool Penalizable { get; set; }
        public Nullable<decimal> MontoPenalizacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
    }
}

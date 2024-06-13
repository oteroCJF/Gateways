using Api.Gateway.Models.Catalogos.DTOs.Incidencias;
using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Agua.DTOs
{
    public class AIncidenciaDto
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int IncidenciaId { get; set; }
        public int TipoId { get; set; }
        public int Pregunta { get; set; }
        public DateTime FechaProgramada { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string HoraProgramada { get; set; }
        public string HoraRealizada { get; set; }
        public int Cantidad { get; set; }
        public string? Observaciones { get; set; }
        public bool Penalizable { get; set; }
        public Nullable<decimal> MontoPenalizacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }

        public AConfiguracionIncidenciaDto ciAgua { get; set; } = new AConfiguracionIncidenciaDto();
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public CTIncidenciaDto Incidencia { get; set; } = new CTIncidenciaDto();
        public CTIAguaDto DIncidencia { get; set; } = new CTIAguaDto();
        public MesDto Mes { get; set; } = new MesDto();
    }
}

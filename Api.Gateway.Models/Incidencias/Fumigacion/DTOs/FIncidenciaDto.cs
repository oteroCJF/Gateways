using Api.Gateway.Models.Catalogos.DTOs.Incidencias;
using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Fumigacion.DTOs
{
    public class FIncidenciaDto
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int IncidenciaId { get; set; }
        public int DIncidenciaId { get; set; }
        public int Pregunta { get; set; }
        public int DiasAtraso { get; set; }
        public int HorasAtraso { get; set; }
        public int MesId { get; set; }
        public bool Penalizable { get; set; }
        public decimal MontoPenalizacion { get; set; }
        public DateTime? FechaProgramada { get; set; }
        public DateTime? FechaRealizada{ get; set; }
        public DateTime? FechaReaparicion { get; set; }
        public string HoraProgramada { get; set; }
        public string HoraRealizada { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public string? Observaciones { get; set; }

        public FConfiguracionIncidenciaDto ciFumigacion { get; set; } = new FConfiguracionIncidenciaDto();
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public CTIncidenciaDto Incidencia { get; set; } = new CTIncidenciaDto();
        public CTIFumigacionDto DIncidencia { get; set; } = new CTIFumigacionDto();
        public MesDto Mes { get; set; } = new MesDto();
    }
}

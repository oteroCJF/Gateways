using Api.Gateway.Models.Catalogos.DTOs.Incidencias;
using Api.Gateway.Models.Catalogos.DTOs.Parametros;
using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Models.Parametrizacion.DTOs;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Comedor.DTOs
{
    public class CIncidenciaDto
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int IncidenciaId { get; set; }
        public int TipoId { get; set; }
        public int Pregunta { get; set; }
        public DateTime FechaIncidencia { get; set; }
        public DateTime FechaProgramada { get; set; }
        public DateTime UltimoDia { get; set; }
        public DateTime FechaRealizada { get; set; }
        public DateTime FechaInventario { get; set; }
        public DateTime FechaNotificacion { get; set; }
        public DateTime FechaAcordadaAdmin { get; set; }
        public DateTime FechaEntrega { get; set; }
        public DateTime FechaLimite { get; set; }
        public bool EntregaEnseres { get; set; }
        public string HoraInicio { get; set; }
        public string HoraReal { get; set; }
        public int Ponderacion { get; set; }
        public int Cantidad { get; set; }
        public string Observaciones { get; set; }
        public bool Penalizable { get; set; }
        public decimal MontoPenalizacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }

        public CConfiguracionIncidenciaDto ciComedor { get; set; } = new CConfiguracionIncidenciaDto();
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public CTIncidenciaDto Incidencia { get; set; } = new CTIncidenciaDto();
        public CTParametroDto Perfil { get; set; } = new CTParametroDto();
        public List<DetalleIncidenciaDto> DIncidencias { get; set; } = new List<DetalleIncidenciaDto>();
    }
}

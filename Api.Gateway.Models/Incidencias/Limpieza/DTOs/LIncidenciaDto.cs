using Api.Gateway.Models.Catalogos.DTOs.Incidencias;
using Api.Gateway.Models.Incidencias.Mensajeria.DTOs;
using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Models.Parametrizacion.DTOs;
using Api.Gateway.Models.Usuarios.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Limpieza.DTOs
{
    public class LIncidenciaDto
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int IncidenciaId { get; set; }
        public int DIncidenciaId { get; set; }
        public Nullable<int> MesId { get; set; }
        public int Pregunta { get; set; }
        public Nullable<DateTime> FechaIncidencia { get; set; }
        public Nullable<int> Inasistencias { get; set; }
        public string? Observaciones { get; set; }
        public Nullable<bool> Penalizable { get; set; }
        public Nullable<decimal> MontoPenalizacion { get; set; }
        public Nullable<DateTime> FechaCreacion { get; set; }
        public Nullable<DateTime> FechaActualizacion { get; set; }
        public Nullable<DateTime> FechaEliminacion { get; set; }

        public LConfiguracionIncidenciaDto ciLimpieza { get; set; } = new LConfiguracionIncidenciaDto();
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public CTILimpiezaDto Incidencia { get; set; } = new CTILimpiezaDto();
        public MesDto Mes { get; set; } = new MesDto();
    }
}

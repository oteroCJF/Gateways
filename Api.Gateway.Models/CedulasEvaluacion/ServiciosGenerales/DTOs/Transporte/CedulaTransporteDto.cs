using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Mensajeria;
using Api.Gateway.Models.Contratos.DTOs;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas;
using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Inmuebles.DTOs.Inmuebles;
using Api.Gateway.Models.LogEntregables.DTOs;
using Api.Gateway.Models.LogsCedulas.DTOs;
using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;

namespace Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Transporte
{
    public class CedulaTransporteDto
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public int InmuebleId { get; set; }
        public InmuebleDto Inmueble { get; set; } //= new InmuebleDto();
        public int MesId { get; set; }
        public MesDto Mes { get; set; } //= new MesDto();
        public int EstatusId { get; set; }
        public EstatusDto Estatus { get; set; } //= new EstatusDto();
        public int Anio { get; set; }
        public string UsuarioId { get; set; }
        public UsuarioDto Usuario { get; set; } //= new UsuarioDto();
        public string Folio { get; set; }
        public bool Bloqueada { get; set; }
        public decimal Calificacion { get; set; }
        public decimal Penalizacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public IEnumerable<TRespuestaDto> respuestas { get; set; } //= new List<MRespuestaDto>();
        public IEnumerable<EntregableDto> entregables { get; set; } //= new List<EntregableDto>();
        public IEnumerable<LogCedulaDto> logs { get; set; } //= new List<LogCedulaDto>();
        public IEnumerable<LogEntregableDto> logsEntregables { get; set; } //= new List<LogEntregableDto>();
        public ContratoDto Contrato { get; set; } = new ContratoDto();

        public int TotalCedulas { get; set; }
    }
}

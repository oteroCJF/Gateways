using Api.Gateway.Models.Catalogos.DTOs.Servicios;
using Api.Gateway.Models.CFDIs.ServiciosBasicos.DTOs;
using Api.Gateway.Models.Entregables.ServiciosBasicos.DTOs;
using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Inmuebles.DTOs.Inmuebles;
using Api.Gateway.Models.LogsEntregables.DTOs;
using Api.Gateway.Models.LogSolicitudes.DTOs;
using Api.Gateway.Models.Meses.DTOs;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;

namespace Api.Gateway.Models.SolicitudesPago.DTOs
{
    public class SolicitudPagoDto
    {
        public int Id { get; set; }
        public int ServicioId { get; set; }
        public string UsuarioId { get; set; }
        public int InmuebleId { get; set; }
        public int Anio { get; set; }
        public int MesId { get; set; }
        public int EstatusId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        //Referencias
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public MesDto Mes { get; set; } = new MesDto();
        public List<CFDISBDto> CFDIs { get; set; } = new List<CFDISBDto>();
        public CTServicioDto Servicio { get; set; } = new CTServicioDto();
        public InmuebleDto Inmueble { get; set; } = new InmuebleDto();
        public EstatusDto Estatus { get; set; } = new EstatusDto();
        public List<FlujoBasicosDto> Flujo { get; set; } = new List<FlujoBasicosDto>();
        public List<EntregableSBDto> Entregables { get; set; } = new List<EntregableSBDto>();
        public List<LogSolicitudDto> LogS { get; set; } = new List<LogSolicitudDto>();
        public List<LogEntregableSBDto> LogE { get; set; } = new List<LogEntregableSBDto>();
    }
}

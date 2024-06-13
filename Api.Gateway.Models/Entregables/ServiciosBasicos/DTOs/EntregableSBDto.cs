using Api.Gateway.Models.Catalogos.DTOs.Entregables;
using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Entregables.ServiciosBasicos.DTOs
{
    public class EntregableSBDto
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; }
        public int EntregableId { get; set; }
        public int EstatusId { get; set; } = 0;
        public string UsuarioId { get; set; } = string.Empty;
        public string Archivo { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }

        public virtual CTEntregableDto Entregable { get; set; } = new CTEntregableDto();
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public EstatusDto Estatus { get; set; } = new EstatusDto();
    }
}

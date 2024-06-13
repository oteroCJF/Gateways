using Api.Gateway.Models.Catalogos.DTOs.Entregables;
using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Parametrizacion.DTOs;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.LogsEntregables.DTOs
{
    public class LogEntregableSBDto
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; }
        public int EstatusId { get; set; }
        public int EntregableId { get; set; }
        public string UsuarioId { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }

        public CTEntregableDto Entregable { get; set; } = new CTEntregableDto();
        public EstatusDto Estatus { get; set; } = new EstatusDto();
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
    }
}

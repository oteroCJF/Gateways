using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Usuarios.DTOs;
using System;

namespace Api.Gateway.Models.LogSolicitudes.DTOs
{
    public class LogSolicitudDto
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; }
        public int EstatusId { get; set; }
        public string UsuarioId { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }

        public EstatusDto Estatus { get; set; } = new EstatusDto();
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
    }
}

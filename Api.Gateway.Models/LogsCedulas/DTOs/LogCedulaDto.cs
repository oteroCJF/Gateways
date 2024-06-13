using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.LogsCedulas.DTOs
{
    public class LogCedulaDto
    {
        public int Id { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int EstatusId { get; set; }
        public string UsuarioId { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }

        public EstatusDto Estatus { get; set; } = new EstatusDto();
        public UsuarioDto Usuario { get; set; } = new UsuarioDto();
    }
}

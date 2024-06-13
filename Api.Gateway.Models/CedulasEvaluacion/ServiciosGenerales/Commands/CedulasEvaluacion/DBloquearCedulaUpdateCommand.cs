using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion
{
    public class DBloquearCedulaUpdateCommand
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int EstatusId { get; set; }
        public int RepositorioId { get; set; }
        public int EFacturaId { get; set; }
        public bool Bloqueada { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}

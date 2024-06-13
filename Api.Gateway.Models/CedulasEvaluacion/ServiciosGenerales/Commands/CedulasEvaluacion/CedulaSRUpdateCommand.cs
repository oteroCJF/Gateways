using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion
{
    public class CedulaSRUpdateCommand
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int EstatusId { get; set; }
        public bool Bloqueada { get; set; }
        public bool Aprobada { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string Observaciones { get; set; }
    }
}

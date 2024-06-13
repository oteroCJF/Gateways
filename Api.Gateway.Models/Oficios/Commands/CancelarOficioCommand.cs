using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Oficios.Commands
{
    public class CancelarOficioCommand
    {
        public int Id { get; set; }
        public int ESucesivoId { get; set; }
        public int EFacturaId { get; set; }
        public int ECedulaId { get; set; }
        public string UsuarioId { get; set; }
        public string Observaciones { get; set; }
    }
}

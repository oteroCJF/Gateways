using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Firmantes.Commands
{
    public class FirmanteCreateCommand
    {
        public string UsuarioId { get; set; }
        public int InmuebleId { get; set; }
        public string Tipo { get; set; }
        public string Escolaridad { get; set; }
    }
}

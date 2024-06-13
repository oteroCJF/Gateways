using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Inmuebles.Commands
{
    public class CreateCommandInmuebleUS
    {
        public int ServicioId { get; set; }
        public int InmuebleId { get; set; }
        public string UsuarioId { get; set; }
    }
}

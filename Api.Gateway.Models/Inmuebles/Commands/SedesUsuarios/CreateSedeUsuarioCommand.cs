using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Inmuebles.Commands.SedesUsuarios
{
    public class CreateSedeUsuarioCommand
    {
        public string UsuarioId { get; set; }
        public int InmuebleId { get; set; }
    }
}

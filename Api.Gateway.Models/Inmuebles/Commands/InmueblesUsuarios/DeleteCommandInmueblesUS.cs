using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Inmuebles.Commands
{
    public class DeleteCommandInmueblesUS
    {
        public string UsuarioId { get; set; }
        public int ServicioId { get; set; }
    }
}

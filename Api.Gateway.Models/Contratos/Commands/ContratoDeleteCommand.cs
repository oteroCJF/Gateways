using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Contratos.Commands
{
    public class ContratoDeleteCommand
    {
        public int Id { get; set; }
        public DateTime FechaEliminacion { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Convenios.Commands
{
    public class ConvenioDeleteCommand
    {
        public int Id { get; set; }
        public DateTime FechaEliminacion { get; set; }
    }
}

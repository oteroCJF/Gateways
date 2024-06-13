using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Oficios.Commands
{
    public class DetalleOficioCreateCommand
    {
        public int ServicioId { get; set; }
        public int OficioId { get; set; }
        public int FacturaId { get; set; }
        public int CedulaId { get; set; }
    }
}

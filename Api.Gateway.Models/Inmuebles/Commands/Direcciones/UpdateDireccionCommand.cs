using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Inmuebles.Commands.Direcciones
{
    public class UpdateDireccionCommand
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Domicilio { get; set; }
        public string Estado { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Catalogos.DTOs.Entregables
{
    public class CTEntregableDto
    {
        public int Id { get; set; }
        public int Orden { get; set; }
        public string Abreviacion { get; set; }
        public string Nombre { get; set; }
    }
}

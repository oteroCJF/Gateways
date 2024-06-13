using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Meses.DTOs
{
    public class MesDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Fondo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Inmuebles.DTOs.InmueblesServicio
{
    public class InmuebleServicioDto
    {
        public int InmuebleId { get; set; }
        public int ServicioId { get; set; }
        public string? TipoServicio { get; set; }
        public string? GrupoSupervision { get; set; }
    }
}

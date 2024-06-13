using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Inmuebles.DTOs.InmueblesUS
{
    public class InmuebleUSDto
    {
        public int ServicioId { get; set; }
        public string UsuarioId { get; set; }
        public int InmuebleId { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}

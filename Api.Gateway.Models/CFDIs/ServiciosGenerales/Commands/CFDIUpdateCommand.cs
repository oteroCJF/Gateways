using Microsoft.AspNetCore.Http;
using System;

namespace Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands
{
    public class CFDIUpdateCommand
    {
        public int Id { get; set; }
        public int Anio { get; set; }
        public string Mes { get; set; }
        public string Inmueble { get; set; }
        public IFormFile PDF { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}

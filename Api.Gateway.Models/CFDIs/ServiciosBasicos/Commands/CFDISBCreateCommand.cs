using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.CFDIs.ServiciosBasicos.Commands
{
    public class CFDISBCreateCommand
    {
        public int Anio { get; set; }
        public int EntregableId { get; set; }
        public int SolicitudId { get; set; }
        public string UsuarioId { get; set; }
        public string Mes { get; set; }
        public IFormFile XML { get; set; }
        public IFormFile PDF { get; set; }
    }
}

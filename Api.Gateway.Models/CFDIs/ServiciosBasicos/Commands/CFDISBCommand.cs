using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.CFDIs.ServiciosBasicos.Commands
{
    public class CFDISBCommand
    {
        public int Anio { get; set; }
        public int SolicitudId { get; set; }
        public string Mes { get; set; }
        public string UsuarioId { get; set; }
        public IFormFile FileXML { get; set; }
        public IFormFile FilePDF { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.CFDIs.ServiciosGenerales.Commands
{
    public class CFDICommand
    {
        public int Anio { get; set; }
        public int InmuebleId { get; set; }
        public int RepositorioId { get; set; }
        public string Mes { get; set; }
        public string UsuarioId { get; set; }
        public string Inmueble { get; set; }
        public string TipoFacturacion { get; set; }
        public IFormFile File { get; set; }
    }
}

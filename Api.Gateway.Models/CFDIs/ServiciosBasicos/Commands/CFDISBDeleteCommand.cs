using Microsoft.AspNetCore.Http;
using System;

namespace Api.Gateway.Models.CFDIs.ServiciosBasicos.Commands
{
    public class CFDISBDeleteCommand
    {
        public int Id { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime FechaEliminacion { get; set; }
        public int Anio { get; set; }
        public string Mes { get; set; }
    }
}

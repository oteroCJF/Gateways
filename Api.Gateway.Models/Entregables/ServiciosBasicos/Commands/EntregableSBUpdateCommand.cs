using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Entregables.ServiciosBasicos.Commands
{
    public class EntregableSBUpdateCommand
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int SolicitudId { get; set; }
        public int EntregableId { get; set; }
        public int EstatusId { get; set; }
        public IFormFile Archivo { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaActualizacion { get; set; }

        public int Anio { get; set; }
        public string Mes { get; set; }
        public string TipoEntregable { get; set; }
    }
}

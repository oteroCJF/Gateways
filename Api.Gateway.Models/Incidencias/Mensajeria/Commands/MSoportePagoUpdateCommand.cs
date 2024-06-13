using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Mensajeria.Commands
{
    public class MSoportePagoUpdateCommand
    {
        public int Anio { get; set; }
        public int MesId { get; set; }
        public string Mes { get; set; }
        public string Folio { get; set; }
        public string UsuarioId { get; set; }
        public IFormFile TXT { get; set; }
        public List<MCedulaSoporteCommand> Cedulas { get; set; } = new List<MCedulaSoporteCommand>();
    }
}

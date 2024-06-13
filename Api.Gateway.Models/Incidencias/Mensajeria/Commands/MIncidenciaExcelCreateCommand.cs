using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Mensajeria.Commands
{
    public class MIncidenciaExcelCreateCommand
    {
        public string Folio { get; set; }
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int IncidenciaId { get; set; }
        public int Pregunta { get; set; }
        public IFormFile Excel { get; set; }
        public List<MCedulaSoporteCommand> CedulaSoporte { get; set; }
    }
}

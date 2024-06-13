using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Transporte.Commands
{
    public class TIncidenciaDeleteCommand
    {

        public int Id { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int Pregunta { get; set; }
        public string TipoIncidencia { get; set; }
    }
}

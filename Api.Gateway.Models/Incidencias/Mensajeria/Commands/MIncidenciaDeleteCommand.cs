using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Mensajeria.Commands
{
    public class MIncidenciaDeleteCommand
    {

        public int Id { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int Pregunta { get; set; }
        public string TipoIncidencia { get; set; }

        //Variables para el acta de Robo/Extravio
        public string Mes { get; set; }
        public string Folio { get; set; }
        public int Anio { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas.Delete
{
    public class EntregableDeleteCommand
    {
        public int CedulaEvaluacionId { get; set; }
        public int EntregableId { get; set; }
        public string UsuarioId { get; set; }
        public string TipoEntregable { get; set; }
        public string Archivo { get; set; }
        public int Anio { get; set; }
        public string Mes { get; set; }
        public string Folio { get; set; }
    }
}

using Api.Gateway.Models.Catalogos.DTOs.Entregables;
using Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Entregables.ServiciosGenerales.Commands.Cedulas
{
    public class DEntregablesCommand
    {
        public int Anio { get; set; }
        public string Path { get; set; }
        public List<int> Meses { get; set; }
        public List<int> Estatus { get; set; }
        public List<int> EntregablesId { get; set; }
        public List<int> InmueblesId { get; set; }
    }
}

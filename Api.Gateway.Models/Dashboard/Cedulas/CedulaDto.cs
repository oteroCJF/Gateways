using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Dashboard.Cedulas
{
    public class CedulaDto
    {
        public int Total { get; set; }
        public int EstatusId { get; set; }
        public string Estatus { get; set; }
        public string Fondo { get; set; }
        public string FondoH { get; set; }
        public Nullable<int> MesId { get; set; }
        public string? Mes { get; set; }
        public Nullable<int> InmuebleId { get; set; }
        public string? Inmueble { get; set; }
        public decimal PorcentajeAvance { get; set; }
    }
}

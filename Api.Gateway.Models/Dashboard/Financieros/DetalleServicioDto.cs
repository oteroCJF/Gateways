using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Dashboard.Financieros
{
    public class DetalleServicioDto
    {
        public int Total { get;set; }
        public int MesId { get;set; }
        public string Mes { get;set; }
        public string Fondo { get;set; }
        public string FondoHexadecimal { get;set; }
        public int EstatusId { get;set; }
        public string Estatus { get;set; }
        public string AreaEjecutora { get;set; }
        public decimal Porcentaje{ get; set; }
    }
}

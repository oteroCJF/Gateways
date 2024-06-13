using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.CFDIs.ServiciosBasicos.DTOs
{
    public class ASGeneralesDto
    {
        public int FacturaId { get; set; }
        public int ClaveInmueble { get; set; }
        public string CuentaPredial { get; set; }
        public string Calle { get; set; }
        public int NoExterior { get; set; }
        public string Colonia { get; set; }
        public string Municipio { get; set; }
        public string Localidad { get; set; }
        public int CP { get; set; }
        public string Pais { get; set; }
        public string Telefono { get; set; }
        public string Observaciones { get; set; }
    }
}

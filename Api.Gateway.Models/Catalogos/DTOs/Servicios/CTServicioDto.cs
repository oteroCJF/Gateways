using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Catalogos.DTOs.Servicios
{
    public class CTServicioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Abreviacion { get; set; }
        public string Descripcion { get; set; }
        public string Icono { get; set; }
        public string Fondo { get; set; }
        public string FondoHexadecimal { get; set; }
        public bool ServicioBasico { get; set; }
    }
}

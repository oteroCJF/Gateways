using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Catalogos.DTOs.Parametros
{
    public class CTParametroDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Tabla { get; set; }
        public string Abreviacion { get; set; }
        public int Orden { get; set; }
        public string Nombre { get; set; }
    }
}

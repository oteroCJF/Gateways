using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Parametrizacion.DTOs
{
    public class ParametroDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Abreviacion { get; set; }
        public string Valor { get; set; }
    }
}

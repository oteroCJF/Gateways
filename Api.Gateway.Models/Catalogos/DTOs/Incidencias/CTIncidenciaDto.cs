using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Catalogos.DTOs.Incidencias
{
    public class CTIncidenciaDto
    {
        public int Id { get; set; }
        public int ServicioId { get; set; }
        public string Abreviacion { get; set; }
        public string Valor { get; set; }
        public DateTime FechaCreacion { get; set; }

    }
}

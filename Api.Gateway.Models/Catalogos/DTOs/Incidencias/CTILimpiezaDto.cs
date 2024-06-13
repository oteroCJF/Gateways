using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Catalogos.DTOs.Incidencias
{
    public class CTILimpiezaDto
    {
        public int Id { get; set; }
        public int IncidenciaId { get; set; }
        public string Tipo { get; set; }
        public string Nombre { get; set; }

        public CTIncidenciaDto Incidencia { get; set; } = new CTIncidenciaDto();
    }
}

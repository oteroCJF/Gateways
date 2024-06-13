using Api.Gateway.Models.Catalogos.DTOs.Parametros;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Catalogos.DTOs.Incidencias
{
    public class CTIComedorDto
    {
        public int Id { get; set; }
        public int IncidenciaId { get; set; }
        public int TipoId { get; set; }
        public string Abreviacion { get; set; }
        public string Nombre { get; set; }

        public CTIncidenciaDto Incidencia { get; set; } = new CTIncidenciaDto();
        public CTParametroDto Tipo { get; set; } = new CTParametroDto();
    }
}

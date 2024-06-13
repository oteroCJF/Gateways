using Api.Gateway.Models.Catalogos.DTOs.Incidencias;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Comedor.DTOs
{
    public class DetalleIncidenciaDto
    {
        public int IncidenciaId { get; set; }
        public int CIncidenciaId { get; set; }
        public decimal MontoPenalizacion { get; set; }
        public CTIComedorDto DIncidencia { get; set; } = new CTIComedorDto();
    }
}

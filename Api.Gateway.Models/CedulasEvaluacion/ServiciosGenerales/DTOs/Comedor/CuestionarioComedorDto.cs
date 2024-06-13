using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.DTOs.Comedor
{
    public class CuestionarioComedorDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Tabla { get; set; }
        public string Abreviacion { get; set; }
        public string Nombre { get; set; }

        public List<CRespuestaDto> Respuestas { get; set; } = new List<CRespuestaDto>();
    }
}

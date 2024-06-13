using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Limpieza.DTOs
{
    public class LConfiguracionIncidenciaDto
    {
        public int Id { get; set; }
        public int Pregunta { get; set; }
        public bool Respuesta { get; set; }
        public bool Detalles { get; set; }
        public bool Obligatorio { get; set; }
        public bool FechaIncidencia { get; set; }
        public bool Inasistencias { get; set; }
        public bool MesSua { get; set; }
        public bool Observaciones { get; set; }
        public string? Ayuda { get; set; }
        public string? RespuestaCedula { get; set; }
    }
}

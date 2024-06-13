using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Agua.DTOs
{
    public class AConfiguracionIncidenciaDto
    {
        public int Id { get; set; }
        public int Pregunta { get; set; }
        public bool Respuesta { get; set; }
        public bool Obligatorio { get; set; }
        public bool Tipo { get; set; }
        public bool FechaProgramada { get; set; }
        public bool FechaEntrega { get; set; }
        public bool HoraProgramada { get; set; }
        public bool HoraRealizada { get; set; }
        public bool Cantidad { get; set; }
        public bool Observaciones { get; set; }
        public string? Ayuda { get; set; }
        public string? RespuestaCedula { get; set; }
    }
}

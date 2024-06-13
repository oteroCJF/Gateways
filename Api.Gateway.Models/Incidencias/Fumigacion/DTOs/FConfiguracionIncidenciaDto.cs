using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Fumigacion.DTOs
{
    public class FConfiguracionIncidenciaDto
    {
        public int Id { get; set; }
        public int Pregunta { get; set; }
        public bool Respuesta { get; set; }
        public bool Detalles { get; set; }
        public bool Obligatorio { get; set; }
        public bool MesSua { get; set; }
        public bool FechaProgramada { get; set; }
        public bool FechaRealizada { get; set; }
        public bool FechaReaparicion { get; set; }
        public bool HoraProgramada { get; set; }
        public bool HoraRealizada { get; set; }
        public bool FaunaNociva { get; set; }
        public bool Observaciones { get; set; }
        public string? RespuestaCedula { get; set; }
        public string? Ayuda { get; set; }
    }
}

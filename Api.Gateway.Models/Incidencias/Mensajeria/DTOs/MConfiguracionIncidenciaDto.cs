using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Incidencias.Mensajeria.DTOs
{
    public class MConfiguracionIncidenciaDto
    {
        public int Id { get; set; }
        public int Pregunta { get; set; }
        public bool Respuesta { get; set; }
        public bool Obligatorio { get; set; }
        public bool FechaProgramada { get; set; }
        public bool FechaEntrega { get; set; }
        public bool NumeroGuia { get; set; }
        public bool CodigoRastreo { get; set; }
        public bool Acuse { get; set; }
        public bool TotalAcuses { get; set; }
        public bool Sobrepeso { get; set; }
        public bool TipoServicio { get; set; }
        public bool TipoIndemnizacion { get; set; }
        public bool Acta { get; set; }
        public bool Escrito { get; set; }
        public bool Comprobante { get; set; }
        public bool Observaciones { get; set; }
        public string? Ayuda { get; set; }
        public string? RespuestaCedula { get; set; }
    }
}

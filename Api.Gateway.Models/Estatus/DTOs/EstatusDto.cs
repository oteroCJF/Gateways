using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Estatus.DTOs
{
    public class EstatusDto
    {
        public int Id { get; set; }
        public string Abreviacion { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string Icono { get; set; }
        public string Fondo { get; set; }
        public string FondoHexadecimal { get; set; }
        public string AreaEjecutora { get; set; }
        public string Prioridad { get; set; } = string.Empty;
        public Nullable<int> OrdenPrioridad { get; set; }
        public Nullable<DateTime> FechaCreacion { get; set; }
        public Nullable<DateTime> FechaActualizacion { get; set; }
        public Nullable<DateTime> FechaEliminacion { get; set; }
    }
}

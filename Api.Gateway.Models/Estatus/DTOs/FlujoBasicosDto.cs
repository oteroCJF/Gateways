using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Estatus.DTOs
{
    public class FlujoBasicosDto
    {
        public int ServicioId { get; set; }
        public int EstatusId { get; set; }
        public int ESucesivoId { get; set; }
        public string? Boton { get; set; }
        public string? Permiso { get; set; }
        public Nullable<bool> Activo { get; set; }
    }
}

using Api.Gateway.Models.Parametrizacion.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Estatus.DTOs.EntregablesEstatus
{
    public class EntregableEstatusDto
    {
        public int ServicioId { get; set; }
        public int EntregableId { get; set; }
        public int EstatusId { get; set; }
        public string Flujo { get; set; }
        public bool Eliminar { get; set; }
    }
}

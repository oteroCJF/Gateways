using Api.Gateway.Models.Catalogos.DTOs.Parametros;
using Api.Gateway.Models.Parametrizacion.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Cuestionarios.DTOs
{
    public class CuestionarioMensualDto
    {
        public int CuestionarioId { get; set; }
        public int Consecutivo { get; set; }
        public int ServicioId { get; set; }
        public int CategoriaId { get; set; }
        public int Anio { get; set; }
        public int MesId { get; set; }
        public int? Ponderacion { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? Formula { get; set; } = string.Empty;
        public decimal Porcentaje { get; set; } = decimal.Zero;
        public System.Nullable<bool> ACLRS { get; set; }
    }
}

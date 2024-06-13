using Api.Gateway.Models.Catalogos.DTOs.Entregables;
using Api.Gateway.Models.Estatus.DTOs;
using Api.Gateway.Models.Parametrizacion.DTOs;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Cedulas
{
    public class EntregableDto
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        public Nullable<int> FacturaId { get; set; }
        public int EntregableId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public string Archivo { get; set; } = string.Empty;
        public int EstatusId { get; set; } = 0;
        public int Orden { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public bool Validado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }

        public CTEntregableDto tipoEntregable { get; set; } = new CTEntregableDto();
        public EstatusDto estatus { get; set; } = new EstatusDto();
        public UsuarioDto usuario { get; set; } = new UsuarioDto();
    }
}

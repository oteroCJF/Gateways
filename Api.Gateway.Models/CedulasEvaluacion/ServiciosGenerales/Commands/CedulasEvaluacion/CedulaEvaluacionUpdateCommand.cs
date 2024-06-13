using Api.Gateway.Models.Catalogos.DTOs.Indemnizaciones;
using Api.Gateway.Models.Catalogos.DTOs.ServiciosContratos;
using Api.Gateway.Models.Contratos.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion
{
    public class CedulaEvaluacionUpdateCommand
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int EstatusId { get; set; }
        public int ServicioId { get; set; }
        public int RepositorioId { get; set; }
        public int EFacturaId { get; set; }
        public int ENotaCreditoId { get; set; }
        public bool Calcula { get; set; }
        public bool Bloqueada { get; set; }
        public bool Elimina { get; set; }
        public string Observaciones { get; set; }
        public decimal UMA { get; set; }
        public string Flujo { get; set; }
        public string Mes { get; set; }
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
        public virtual List<CTIndemnizacionDto> Indemnizaciones { get; set; } = new List<CTIndemnizacionDto>();
    }
}

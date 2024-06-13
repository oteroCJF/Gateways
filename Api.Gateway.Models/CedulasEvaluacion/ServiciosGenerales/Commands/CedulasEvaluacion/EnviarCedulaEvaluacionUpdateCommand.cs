using Api.Gateway.Models.Catalogos.DTOs.Indemnizaciones;
using Api.Gateway.Models.Contratos.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.CedulasEvaluacion.ServiciosGenerales.Commands.CedulasEvaluacion
{
    public class EnviarCedulaEvaluacionUpdateCommand
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int EstatusId { get; set; }
        public int RepositorioId { get; set; }
        public int EFacturaId { get; set; }
        public int ENotaCreditoId { get; set; }
        public bool Calcula { get; set; }
        public string Estatus { get; set; }
        public string Observaciones { get; set; }
        public decimal UMA { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public virtual List<ServicioContratoDto> Penalizacion { get; set; } = new List<ServicioContratoDto>();
        public virtual List<CTIndemnizacionDto> Indemnizaciones { get; set; } = new List<CTIndemnizacionDto>();
    }
}

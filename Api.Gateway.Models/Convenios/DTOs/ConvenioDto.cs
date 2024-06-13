using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Contratos;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Convenios.DTOs
{
    public class ConvenioDto
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public string UsuarioId { get; set; }
        public string NoConvenio { get; set; }
        public DateTime InicioVigencia { get; set; }
        public DateTime FinVigencia { get; set; }
        public decimal MontoMin { get; set; }
        public decimal MontoMax { get; set; }
        public int VolumetriaMin { get; set; }
        public int VolumetriaMax { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaFirmaConvenio { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }

        public virtual UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public virtual List<EContratoDto> EntregablesConvenio { get; set; } = new List<EContratoDto>();
        public virtual List<RubroConvenioDto> Rubros { get; set; } = new List<RubroConvenioDto>();
    }
}

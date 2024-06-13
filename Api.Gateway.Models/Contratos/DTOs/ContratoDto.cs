using Api.Gateway.Models.Convenios.DTOs;
using Api.Gateway.Models.Entregables.ServiciosGenerales.DTOs.Contratos;
using Api.Gateway.Models.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Api.Gateway.Models.Contratos.DTOs
{
    public class ContratoDto
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public string NoContrato { get; set; }
        public string Empresa { get; set; }
        public string RFC { get; set; }
        public string Direccion { get; set; }
        public decimal MontoMin { get; set; }
        public decimal MontoMax { get; set; }
        public int VolumetriaMin { get; set; }
        public int VolumetriaMax { get; set; }
        public string Representante { get; set; }
        public DateTime InicioVigencia { get; set; }
        public DateTime FinVigencia { get; set; }
        public DateTime FechaFirmaContrato { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }

        public virtual UsuarioDto Usuario { get; set; } = new UsuarioDto();
        public virtual List<ConvenioDto> Convenios { get; set; } = new List<ConvenioDto>();
        public virtual List<EContratoDto> EntregablesContrato { get; set; } = new List<EContratoDto>();
        public virtual List<ServicioContratoDto> ServiciosContrato { get; set; } = new List<ServicioContratoDto>();
    }
}
